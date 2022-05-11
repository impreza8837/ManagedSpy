using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace ManagedSpy {
    /// <summary>
    /// This dialog allows the user to filter events.
    /// Note that this could be improved to actually show _all_ events.
    /// To do this, you would have to look at TypeDescriptor.GetEvents of
    /// the ComponentType property of the ControlProxy.
    /// </summary>
    public partial class EventFilterDialog : Form {
        internal EventFilterList EventList { get; set; } = new EventFilterList();

        public EventFilterDialog() {
            InitializeComponent();
            dataGridView1.DataSource = EventList;
            dataGridView1.Columns[0].Width = 47;
            dataGridView1.Columns[1].Width = 170;
        }

        private void ButtonOK_Click(object sender, EventArgs e) {
            Close();
        }

        private void DataGridView1_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == (char)Keys.Space) {
                ToggleSelectedCells();
            }
        }

        private void ToggleSelectedCells() {
            if (dataGridView1.SelectedCells.Count > 0) {
                bool newValue = !EventList[dataGridView1.SelectedCells[0].RowIndex].Display;
                foreach (DataGridViewCell cell in dataGridView1.SelectedCells) {
                    if (cell.ColumnIndex == 0) {
                        cell.Value = newValue;
                    }
                }
            }
        }

        private void DataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e) {
            ToggleSelectedCells();
        }
    }

    class EventFilterList : List<EventFilterLine> {
        public EventFilterList() {
            EventDescriptorCollection edColl = TypeDescriptor.GetEvents(typeof(Control));
            edColl = edColl.Sort();

            Add(new EventFilterLine("Custom Events", true));
            for (int i = 0; i < edColl.Count; i++) {
                Add(new EventFilterLine(edColl[i].Name, true));
            }
        }

        public EventFilterLine this[string eventName] {
            get {
                foreach (EventFilterLine line in this) {
                    if (line.EventName == eventName) {
                        return line;
                    }
                }
                return this[0]; //custom
            }
        }
    }

    class EventFilterLine {
        public EventFilterLine(string eventName, bool isChecked) {
            EventName = eventName;
            Display = isChecked;
        }

        public string EventName {
            get;
        }

        public bool Display {
            get; set;
        }
    }
}