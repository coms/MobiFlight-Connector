﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MobiFlight.UI.Panels.Settings
{
    public partial class GeneralPanel : UserControl
    {
        public GeneralPanel()
        {
            InitializeComponent();
        }

        public static List<string> GetAvailableLogSeverityLevels()
        {
            return Enum.GetNames(typeof(LogSeverity)).ToList();
        }

        protected void InitializeLanguageComboBox()
        {
            languageComboBox.Items.Clear();

            List<ListItem> languageOptions = new List<ListItem>();
            languageOptions.Add(new ListItem() { Value = "", Label = "System Default" });
            languageOptions.Add(new ListItem() { Value = "en-US", Label = "English" });
            languageOptions.Add(new ListItem() { Value = "de-DE", Label = "Deutsch" });

            languageComboBox.DataSource = languageOptions;
            languageComboBox.DisplayMember = "Label";
            languageComboBox.ValueMember = "Value";
            languageComboBox.SelectedIndex = 0;
        }

        protected void InitializeRemoteConnectionComboBox()
        {
            remoteConnectionComboBox.Items.Clear();

            var languageOptions = new List<ListItem>
            {
                new ListItem() { Value = "None", Label = "Disabled" },
                new ListItem() { Value = FlightSimType.MSFS2020.ToString(), Label = FlightSimType.MSFS2020.ToString() },
                new ListItem() { Value = FlightSimType.XPLANE.ToString(), Label = FlightSimType.XPLANE.ToString() }
            };

            remoteConnectionComboBox.DataSource = languageOptions;
            remoteConnectionComboBox.DisplayMember = "Label";
            remoteConnectionComboBox.ValueMember = "Value";
            remoteConnectionComboBox.SelectedIndex = 0;
        }

        protected void InitializeLogLevelComboBox()
        {
            List<ListItem> LogLevels = new List<ListItem>();
            foreach (var level in GetAvailableLogSeverityLevels())
            {
                LogLevels.Add(new ListItem() { Value = level, Label = level });
            }
            logLevelComboBox.DataSource = LogLevels;
            logLevelComboBox.DisplayMember = "Label";
            logLevelComboBox.ValueMember = "Value";
            logLevelComboBox.SelectedIndex = 0;
        }

        public void loadSettings()
        {
            //
            // TAB General
            //
            // Recent Files max count
            recentFilesNumericUpDown.Value = Properties.Settings.Default.RecentFilesMaxCount;

            InitializeLanguageComboBox();

            // TestMode speed
            // (1s) 0 - 4 (50ms)
            testModeSpeedTrackBar.Value = 0;
            if (Properties.Settings.Default.TestTimerInterval == 500) testModeSpeedTrackBar.Value = 1;
            else if (Properties.Settings.Default.TestTimerInterval == 250) testModeSpeedTrackBar.Value = 2;
            else if (Properties.Settings.Default.TestTimerInterval == 125) testModeSpeedTrackBar.Value = 3;
            else if (Properties.Settings.Default.TestTimerInterval == 50) testModeSpeedTrackBar.Value = 4;

            // Config Execution Speed
            int ExecutionSpeedInTicks = (int)Math.Floor(Properties.Settings.Default.PollInterval / 25.0);
            // Maximum is 10, and Minimum is 1
            fsuipcPollIntervalTrackBar.Value = Math.Max(fsuipcPollIntervalTrackBar.Minimum, (fsuipcPollIntervalTrackBar.Maximum + fsuipcPollIntervalTrackBar.Minimum - ExecutionSpeedInTicks));

            // Debug Mode
            InitializeLogLevelComboBox();
            logLevelCheckBox.Checked = Properties.Settings.Default.LogEnabled;
            ComboBoxHelper.SetSelectedItem(logLevelComboBox, Properties.Settings.Default.LogLevel);
            LogJoystickAxisCheckBox.Checked = Properties.Settings.Default.LogJoystickAxis;

            // System Language
            languageComboBox.SelectedValue = Properties.Settings.Default.Language;

            // Beta Versions
            BetaUpdateCheckBox.Checked = Properties.Settings.Default.BetaUpdates;

            // Community Feedback Program
            CommunityFeedbackCheckBox.Checked = Properties.Settings.Default.CommunityFeedback;

            // Auto-retrigger
            autoRetriggerCheckBox.Checked = Properties.Settings.Default.AutoRetrigger;

            InitializeRemoteConnectionComboBox();
            ComboBoxHelper.SetSelectedItem(remoteConnectionComboBox, Properties.Settings.Default.RemoteConnection);
            remoteConnectionTextBox.Text = Properties.Settings.Default.RemoteIpAddress;
        }

        public void saveSettings()
        {
            if (testModeSpeedTrackBar.Value == 0) Properties.Settings.Default.TestTimerInterval = 1000;
            else if (testModeSpeedTrackBar.Value == 1) Properties.Settings.Default.TestTimerInterval = 500;
            else if (testModeSpeedTrackBar.Value == 2) Properties.Settings.Default.TestTimerInterval = 250;
            else if (testModeSpeedTrackBar.Value == 3) Properties.Settings.Default.TestTimerInterval = 125;
            else Properties.Settings.Default.TestTimerInterval = 50;

            // Config Execution Speed
            Properties.Settings.Default.PollInterval = (int)(((fsuipcPollIntervalTrackBar.Maximum + fsuipcPollIntervalTrackBar.Minimum) - fsuipcPollIntervalTrackBar.Value) * 25);

            // Recent Files max count
            Properties.Settings.Default.RecentFilesMaxCount = (int)recentFilesNumericUpDown.Value;

            // log settings
            Properties.Settings.Default.LogEnabled = logLevelCheckBox.Checked;
            Properties.Settings.Default.LogLevel = (logLevelComboBox.SelectedItem as ListItem).Value;
            Log.Instance.Enabled = logLevelCheckBox.Checked;
            Log.Instance.Severity = (LogSeverity)Enum.Parse(typeof(LogSeverity), Properties.Settings.Default.LogLevel);

            Properties.Settings.Default.LogJoystickAxis = LogJoystickAxisCheckBox.Checked;
            Log.Instance.LogJoystickAxis = LogJoystickAxisCheckBox.Checked;

            // System Language
            Properties.Settings.Default.Language = languageComboBox.SelectedValue.ToString();

            // Beta Versions
            Properties.Settings.Default.BetaUpdates = BetaUpdateCheckBox.Checked;

            // Community Feedback Program
            Properties.Settings.Default.CommunityFeedback = CommunityFeedbackCheckBox.Checked;

            // Auto-retrigger
            Properties.Settings.Default.AutoRetrigger = autoRetriggerCheckBox.Checked;

            // Remote Connection 
            Properties.Settings.Default.RemoteConnection = remoteConnectionComboBox.SelectedValue.ToString();
            Properties.Settings.Default.RemoteIpAddress = remoteConnectionTextBox.Text;
        }
    }
}
