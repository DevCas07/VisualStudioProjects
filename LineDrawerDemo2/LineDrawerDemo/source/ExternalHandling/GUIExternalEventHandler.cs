using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace LineDrawerDemo
{
    /// <summary>
    /// Declares a public delegate that can interact throught the entire namespace that handles the external events
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void GUIExternalEventHandler(object sender, EventArgs e);

    /// <summary>
    /// The purpose of this class is to be able to declare the logic of events in form1.cs but still be able to call that code where ever needed without needing to inherit form1.cs further down the hierarchy
    /// </summary>
    public class GUIExternalEvents
    {
        private static GUIExternalEvents instance;
        private static readonly object _lock = new object(); //Lock object for thread safety 

        public event GUIExternalEventHandler EventUpdateFormObjects;
        public event GUIExternalEventHandler EventUpdateFormTreeViews;
        public event GUIExternalEventHandler EventResetFormParameters;
        public event GUIExternalEventHandler EventClearTreeViews;

        private GUIExternalEvents() { }

        public static GUIExternalEvents GetInstance() //Create a single accessable singleton instance
        {
            lock (_lock)
            {
                if (instance == null)
                {
                    instance = new GUIExternalEvents();
                }
                return instance;
            }
        }
        //
        // Declare invoking of the events
        //
        protected virtual void OnUpdateFormObjects(EventArgs e)
        {
            EventUpdateFormObjects.Invoke(this, e);
        }
        protected virtual void OnUpdateFormTreeViews(EventArgs e)
        {
            
            EventUpdateFormTreeViews.Invoke(this, e);
        }
        protected virtual void OnEventResetFormParameters(EventArgs e)
        {
            EventUpdateFormTreeViews.Invoke(this, e);
        }
        protected virtual void OnEventClearTreeViews(EventArgs e)
        {
            EventClearTreeViews.Invoke(this, e);
        }

        //
        // Callable methods that trigger the events
        //
        public void UpdateFormObjects()
        {
            OnUpdateFormObjects(EventArgs.Empty);
        }
        
        public void UpdateFormTreeViews()
        {
            OnUpdateFormTreeViews(EventArgs.Empty);
        }
        public void ResetFormParameters()
        {
            OnEventResetFormParameters(EventArgs.Empty);
        }
        public void ClearTreeViews()
        {
            OnEventClearTreeViews(EventArgs.Empty);
        }
    }
}
