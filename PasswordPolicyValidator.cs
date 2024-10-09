using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Summary description for PasswordPolicyValidator
/// </summary>

namespace ParsElecom.PasswordPolicy
{
    [ToolboxData("<{0}:PasswordPolicyValidator runat=server></{0}:PasswordPolicyValidator>")]
    public class PasswordPolicyValidator : BaseValidator
    {
        #region Member Variables
        string _MessageLabel = string.Empty;
        #endregion

        #region Properties
        public string UnicodeCharSetRanges { get { return ViewState["UnicodeCharSetRanges"].ToString(); } set { ViewState["UnicodeCharSetRanges"] = value; } }

        public int MinPasswordLength { get { return int.Parse(ViewState["MinPasswordLength"].ToString()); } set { ViewState["MinPasswordLength"] = value; } }
        public int MinNoOfUpperCaseChars { get { return int.Parse(ViewState["MinNoOfUpperCaseChars"].ToString()); } set { ViewState["MinNoOfUpperCaseChars"] = value; } }
        public int MinNoOfLowerCaseChars { get { return int.Parse(ViewState["MinNoOfLowerCaseChars"].ToString()); ; } set { ViewState["MinNoOfLowerCaseChars"] = value; } }
        public int MinNoOfUniCaseChars { get { return int.Parse(ViewState["MinNoOfUniCaseChars"].ToString()); ; } set { ViewState["MinNoOfUniCaseChars"] = value; } }
        public int MinNoOfNumbers { get { return int.Parse(ViewState["MinNoOfNumbers"].ToString()); ; } set { ViewState["MinNoOfNumbers"] = value; } }
        public int MinNoOfSymbols { get { return int.Parse(ViewState["MinNoOfSymbols"].ToString()); ; } set { ViewState["MinNoOfSymbols"] = value; } }
        public int MaxNoOfAllowedRepetitions { get { return int.Parse(ViewState["MaxNoOfAllowedRepetitions"].ToString()); } set { ViewState["MaxNoOfAllowedRepetitions"] = value; } }
        public string DisallowedChars { get { return ViewState["DisallowedChars"].ToString(); } set { ViewState["DisallowedChars"] = value; } }
        public string UserNameControlID { get { return ViewState["UserNameControlID"].ToString(); } set { ViewState["UserNameControlID"] = value; } }
        public string ValidationExpression { get { return ViewState["ValidationExpression"].ToString(); } set { ViewState["ValidationExpression"] = value; } }

        public string MinPasswordLengthStrength { get { return ViewState["MinPasswordLengthStrength"].ToString(); } set { ViewState["MinPasswordLengthStrength"] = value; } }
        public string MinNoOfUpperCaseCharsStrength { get { return ViewState["MinNoOfUpperCaseCharsStrength"].ToString(); } set { ViewState["MinNoOfUpperCaseCharsStrength"] = value; } }
        public string MinNoOfLowerCaseCharsStrength { get { return ViewState["MinNoOfLowerCaseCharsStrength"].ToString(); } set { ViewState["MinNoOfLowerCaseCharsStrength"] = value; } }
        public string MinNoOfNumbersStrength { get { return ViewState["MinNoOfNumbersStrength"].ToString(); } set { ViewState["MinNoOfNumbersStrength"] = value; } }
        public string MinNoOfSymbolsStrength { get { return ViewState["MinNoOfSymbolsStrength"].ToString(); } set { ViewState["MinNoOfSymbolsStrength"] = value; } }
        public string MaxNoOfAllowedRepetitionsStrength { get { return ViewState["MaxNoOfAllowedRepetitionsStrength"].ToString(); } set { ViewState["MaxNoOfAllowedRepetitionsStrength"] = value; } }

        public string StrengthCategories { get { return ViewState["StrengthCategories"].ToString(); } set { ViewState["StrengthCategories"] = value; } }
        public string StrengthColours { get { return ViewState["StrengthColours"].ToString(); } set { ViewState["StrengthColours"] = value; } }

        #endregion

        #region Constructors
        public PasswordPolicyValidator()
        {
            ViewState["UnicodeCharSetRanges"] = string.Empty;

            ViewState["MinPasswordLength"] = -1;
            ViewState["MinNoOfLowerCaseChars"] = -1;
            ViewState["MinNoOfUpperCaseChars"] = -1;
            ViewState["MinNoOfUniCaseChars"] = -1;
            ViewState["MinNoOfNumbers"] = -1;
            ViewState["MinNoOfSymbols"] = -1;
            ViewState["MaxNoOfAllowedRepetitions"] = -1;
            ViewState["DisallowedChars"] = string.Empty;
            ViewState["UserNameControlID"] = string.Empty;

            ViewState["StrengthCategories"] = string.Empty;
            ViewState["StrengthColours"] = string.Empty;            
            ViewState["MinPasswordLengthStrength"] = string.Empty;
            ViewState["MinNoOfUpperCaseCharsStrength"] = string.Empty;
            ViewState["MinNoOfLowerCaseCharsStrength"] = string.Empty;
            ViewState["MinNoOfNumbersStrength"] = string.Empty;
            ViewState["MinNoOfSymbolsStrength"] = string.Empty;
            ViewState["MaxNoOfAllowedRepetitionsStrength"] = string.Empty;

            ViewState["ValidationExpression"] = string.Empty;

            this.Load += new EventHandler(PasswordPolicyValidator_Load);
            this.PreRender += new EventHandler(PasswordPolicyValidator_PreRender);
        }
        #endregion

        #region Event Handlers
        protected override bool EvaluateIsValid()
        {
            return true;
        }

        protected override void  Render(HtmlTextWriter writer)
        {             
 	         base.Render(writer);             
             writer.Write("<label id='" + _MessageLabel + "'></label>");
        }

        void PasswordPolicyValidator_PreRender(object sender, EventArgs e)
        {
            if (this.EnableClientScript)
            {
                this.Page.ClientScript.RegisterExpandoAttribute(this.ClientID,
                "evaluationfunction", this.GetType().Name +
                "Validate" + this.ClientID);

                this.Page.ClientScript.RegisterExpandoAttribute(this.ClientID,
                "validationexpression", this.ValidationExpression);
            }
        }

        void PasswordPolicyValidator_Load(object sender, EventArgs e)
        {
            try
            {
                _MessageLabel = "lblMessage" + this.ClientID;

                StrengthSpecificationsArray();

                string scriptLocation = Page.ClientScript.GetWebResourceUrl(this.GetType(), "ParsElecom.PasswordPolicy.JScript.js");
                Page.ClientScript.RegisterClientScriptInclude("ParsElecom.PasswordPolicy.JScript.js", scriptLocation);

                this.ValidationExpression = new PasswordPolicy
                {
                    UnicodeCharSetRanges = UnicodeCharSetRanges,
                    MaxNoOfAllowedRepetitions = MaxNoOfAllowedRepetitions,
                    MinNoOfLowerCaseChars = MinNoOfLowerCaseChars,
                    MinNoOfUniCaseChars = MinNoOfUniCaseChars,
                    MinNoOfNumbers = MinNoOfNumbers,
                    MinNoOfUpperCaseChars = MinNoOfUpperCaseChars,
                    MinNoOfSymbols = MinNoOfSymbols,
                    MinPasswordLength = MinPasswordLength,
                    DisallowedChars = DisallowedChars
                }.GetExpression();

                string UserNameControlClientID = string.Empty;

                if (!String.IsNullOrEmpty(UserNameControlID))
                {
                    Control ctrl = this.NamingContainer.FindControl(UserNameControlID);

                    if (ctrl != null)
                        UserNameControlClientID = this.NamingContainer.FindControl(UserNameControlID).ClientID;
                }

                this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Validation", "function PasswordPolicyValidatorValidate" + this.ClientID + "(){ var val = document.getElementById('" + this.ClientID + "'); var arr = GetStrengthArray" + this.ClientID + "(); var arrCategories = GetCategoriesArray" + this.ClientID + "(); var strength = GetStrength(arr, arrCategories, '" + this.ClientID + "',GetUnicodeCharSetRangesArray" + this.ClientID + "()); ShowStrengthColour(strength, arrCategories, GetColoursArray" + this.ClientID + "(), '" + _MessageLabel + "');  return PasswordPolicyValidatorValidate('" + this.ClientID + "', '" + UserNameControlClientID + "'); }", true);
            }
            catch { }
        }
        #endregion

        #region Functions
        public void StrengthSpecificationsArray()
        {
            PropertyInfo[] propertyInfos;
            propertyInfos = typeof(PasswordPolicyValidator).GetProperties();

            List<PropertyInfo> lstPropertyInfos = propertyInfos.ToList().Where(pi => pi.Name.EndsWith("Strength") && pi.GetValue(this, null).ToString().Trim().Length > 0).ToList();

            int propertyCount = lstPropertyInfos.Count;

            string js = "function GetStrengthArray" + this.ClientID + "(){var strengthArray = new Array(" + propertyCount + ");" +
                           "for (i=0; i <" + propertyCount + "; i++){strengthArray[i]=new Array(" + StrengthCategories.Split(',').Count() +");}";

            int i = 0;


            for (i = 0; i < propertyCount; i++)
            {
                PropertyInfo pi = lstPropertyInfos[i];

                js = js + @"strengthArray[" + i + "][1]='" + pi.Name.Substring(0, pi.Name.IndexOf("Strength")) + "';";

                for (int j = 2; j < StrengthCategories.Split(',').Count() + 2; j++)
                {
                    js = js + @"strengthArray[" + i.ToString() + "][" + j.ToString() + "]='" + pi.GetValue(this, null).ToString().Split(',')[j - 2].Trim() + "';";
                } 
            }

            js += " return strengthArray;}";

            int NoOfCategories = StrengthCategories.Split(',').Count();

            js += "function GetCategoriesArray" + this.ClientID + "() { var arr = new Array(" + NoOfCategories + ");";
            for (i = 0; i < NoOfCategories; i++)
            {
                js += @"arr[" + i.ToString() + "]='" + StrengthCategories.Split(',')[i].Trim() + "';";
            }

            js += @" return arr; }";

            int NoOfColours = StrengthColours.Split(',').Count();

            js = js + "function GetColoursArray" + this.ClientID + "() { var arr = new Array(" + NoOfColours + ");";
            for (i = 0; i < NoOfColours; i++)
            {
                js = js + @"arr[" + i.ToString() + "]='" + StrengthColours.Split(',')[i].Trim() + "';";
            }

            js += @" return arr; }";

            int NoOfCases = UnicodeCharSetRanges.Split(',').Count();

            js += "function GetUnicodeCharSetRangesArray" + this.ClientID + "() { var arr = new Array(" + NoOfCases + ");";

            for (i = 0; i < NoOfCases; i++)
            {
                js += @"arr[" + i.ToString() + "]='" + UnicodeCharSetRanges.Split(',')[i].Trim() + "';";
            }

            js += @" return arr; }";

            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "StrengthArray", js, true);
        }
        #endregion
    }
}