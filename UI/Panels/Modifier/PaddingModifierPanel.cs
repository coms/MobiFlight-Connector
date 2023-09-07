﻿using MobiFlight.Modifier;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Padding = MobiFlight.Modifier.Padding;

namespace MobiFlight.UI.Panels.Modifier
{
    public partial class PaddingModifierPanel : UserControl, IModifierConfigPanel
    {
        public event EventHandler ModifierChanged;

        public PaddingModifierPanel()
        {
            InitializeComponent();

            comboBox1.Leave += control_Leave;
            comboBoxCharacter.Leave += control_Leave;
            comboBoxCharacter.Leave += control_Leave;
            textBoxLength.Leave += control_Leave;
            textBoxLength.TextChanged += control_Leave;

            comboBox1.Items.Clear();
            var items = new List<ListItem<Padding.PaddingDirection>>() {
                new ListItem<Padding.PaddingDirection>()
                {
                    Value = MobiFlight.Modifier.Padding.PaddingDirection.Left,
                    Label = MobiFlight.Modifier.Padding.PaddingDirection.Left.ToString()
                },
                new ListItem<Padding.PaddingDirection>()
                {
                    Value = MobiFlight.Modifier.Padding.PaddingDirection.Right,
                    Label = MobiFlight.Modifier.Padding.PaddingDirection.Right.ToString()
                }
            };
            comboBox1.DataSource = items;
            comboBox1.ValueMember = "Value";
            comboBox1.DisplayMember = "Label";

            var blankOptions = new List<ListItem>()
            {
                new ListItem() { Label = "[Space]", Value = " " },
                new ListItem() { Label = "0", Value = "0" },
                new ListItem() { Label = "1", Value = "1" },
            };

            comboBoxCharacter.DataSource = blankOptions;
            comboBoxCharacter.ValueMember = "Value";
            comboBoxCharacter.DisplayMember = "Label";

        }

        public void fromConfig (ModifierBase c)
        {
            var config = c as Padding;

            if (config == null) return;

            if (!ComboBoxHelper.SetSelectedItemByValue(comboBoxCharacter, config.Character.ToString()))
            {
                var list = (comboBoxCharacter.DataSource as List<ListItem>);
                list.Add(new ListItem() { Label = config.Character.ToString(), Value = config.Character.ToString() });
                comboBoxCharacter.DataSource = null;
                comboBoxCharacter.DataSource = list;
                comboBoxCharacter.ValueMember = "Value";
                comboBoxCharacter.DisplayMember = "Label";
                comboBoxCharacter.SelectedItem = config.Character;
                ComboBoxHelper.SetSelectedItemByValue(comboBoxCharacter, config.Character.ToString());
            };

            textBoxLength.Text = config.Length.ToString();
            ComboBoxHelper.SetSelectedItem(comboBox1, config.Direction.ToString());
        }

        public ModifierBase toConfig () {
            int.TryParse(textBoxLength.Text, out int length);

            var character = comboBoxCharacter.Text;
            if ((comboBoxCharacter.SelectedItem as ListItem)?.Value != null) {
                character = ((comboBoxCharacter.SelectedItem as ListItem).Value)[0].ToString();
            }

            return new Padding()
            {
                Active = true,
                Direction = (Padding.PaddingDirection)comboBox1.SelectedValue,
                Character = character[0],
                Length = length
            };
        }

        private void control_Leave(object sender, EventArgs e)
        {
            ModifierChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
