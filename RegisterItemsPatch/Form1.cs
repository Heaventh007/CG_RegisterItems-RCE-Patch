using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using XDevkit;
using JRPC_Client;

namespace RegisterItemsPatch {

    public partial class Form1 : Form {

        // Instance for our console.
        private IXboxConsole Xbox360 = null;

        // Actual patch bytes.
        byte[] CG_RegisterItems_Patch = {
            0x91, 0x41, 0x01, 0x50, 0x3C, 0x00, 0x82, 0x26, 0x60, 0x00, 0x28, 0xD8,
            0x7C, 0x09, 0x03, 0xA6, 0x4E, 0x80, 0x04, 0x21, 0x2F, 0x03, 0x00, 0x00,
            0x41, 0x9A, 0x00, 0x3C, 0x7C, 0x7F, 0x1B, 0x78, 0x3C, 0x00, 0x82, 0x49,
            0x60, 0x00, 0xE1, 0x58, 0x7C, 0x09, 0x03, 0xA6, 0x4E, 0x80, 0x04, 0x21,
            0x2B, 0x03, 0x01, 0x00, 0x40, 0x98, 0x00, 0x20, 0x7F, 0xE3, 0xFB, 0x78,
            0x39, 0x21, 0x00, 0x50, 0x7D, 0x63, 0x48, 0x50, 0x3C, 0x00, 0x82, 0x24,
            0x60, 0x00, 0xB1, 0x9C, 0x7C, 0x09, 0x03, 0xA6, 0x4E, 0x80, 0x04, 0x20,
            0x3C, 0x00, 0x82, 0x24, 0x60, 0x00, 0xB2, 0x40, 0x7C, 0x09, 0x03, 0xA6,
            0x4E, 0x80, 0x04, 0x20
        };

        // Patch in jump to our patch.
        byte[] CG_RegisterItems_Jump = {
            0x3C, 0x00, 0x82, 0x94,
            0x60, 0x00, 0x53, 0x48,
            0x7C, 0x09, 0x03, 0xA6,
            0x4E, 0x80, 0x04, 0x20
        };

        public Form1() => InitializeComponent();

        private void button1_Click(object sender, EventArgs e)
            => MessageBox.Show("Exploit patched by Heaventh.");

        private void button3_Click(object sender, EventArgs e) {
            try {
                // Attempt to establish a connection.
                if (Xbox360.Connect(out Xbox360)) {
                    MessageBox.Show("Successfully connected to console!");
                    return;
                }

                MessageBox.Show("Failed to connect to console!");
            } catch (Exception ex) {
                // Exception occured, let's just show the error...
                MessageBox.Show("An error occured while trying to connect to the console: " + ex.ToString());
            }
        }

        private void button2_Click(object sender, EventArgs e) {
            // Write our patch into memory.
            Xbox360.SetMemory(0x82945348, CG_RegisterItems_Patch);
            // Write our jump into memory.
            Xbox360.SetMemory(0x8224B18C, CG_RegisterItems_Jump);
        }
    }

}
