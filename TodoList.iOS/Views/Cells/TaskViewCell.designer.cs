﻿// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace TodoList.iOS.Views.Cells
{
    [Register ("TaskViewCell")]
    partial class TaskViewCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel TaskNameLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISwitch TaskStatusSwitch { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (TaskNameLabel != null) {
                TaskNameLabel.Dispose ();
                TaskNameLabel = null;
            }

            if (TaskStatusSwitch != null) {
                TaskStatusSwitch.Dispose ();
                TaskStatusSwitch = null;
            }
        }
    }
}