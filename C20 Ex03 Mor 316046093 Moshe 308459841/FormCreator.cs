using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace C20_Ex03_Mor_316046093_Moshe_308459841
{
    public class FormCreator
    {
        public enum eSupportedForms
        {
            Login
        }

        public static Form CreateForm(eSupportedForms i_FormType)
        {
            Form newForm = null;
            switch (i_FormType)
            {
                case eSupportedForms.Login:
                    {
                        newForm = new FormLogin();
                        break;
                    }
            }

            return newForm;
        }
    }
}