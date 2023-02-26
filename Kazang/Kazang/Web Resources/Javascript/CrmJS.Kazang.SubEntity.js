if (typeof CrmJS === "undefined") { CrmJS = { __namespace: true } }
if (typeof CrmJS.Kazang === "undefined") { CrmJS.Kazang = { __namespace: true } }
if (typeof CrmJS.Kazang.SubEntity === "undefined") {
    CrmJS.Kazang.SubEntity = {
    Form: {
      Main: {
        onLoad: function (context) {
		  debugger;
          setTimeout(function () {
            entityJS.Form.Main.attachEvents();
            entityJS.Form.insertDefaults();
            entityJS.Form.Main.init();
            entityJS.Form.Main.businessRules(context, true);
            entityJS.Form.initControlViews();
          }, 1);
        },

        onSave: function (executionContext) {
            debugger;
            var formContext = executionContext.getFormContext();
            CrmJS.Kazang.SubEntity.Form.Functions.disableSubFields(formContext);
        },

        attachEvents: function () {
          
        },
		
        businessRules: function (executionContext, onLoad) { 
            debugger;
            var formContext = executionContext.getFormContext();
            CrmJS.Kazang.SubEntity.Form.Functions.disableSubFields(formContext);
        },

        init: function () {   
        },
        __namespace: true
      },

      initControlViews: function () {
      },

      cancelSave: function () {
		debugger;
        return !confirm("Please confirm that all information has been updated accordingly.");
      },

      insertDefaults: function () { 
      },

	  Functions: {
        sampleMethod: function () {
		alert("Sample Call Invoked");
		///Accessed by all forms
		},

        disableSubFields: function (formContext) {
            debugger;
            if (formContext.getAttribute("new_authorize").getValue() === true) {
                //General
                formContext.getControl("new_name").setDisabled(true);
                formContext.getControl("createdon").setDisabled(true);
                formContext.getControl("ownerid").setDisabled(true);
                formContext.getControl("new_master").setDisabled(true);
                formContext.getControl("new_authorize").setDisabled(true);
                formContext.getControl("new_authorisedby").setDisabled(true);

                //Authorized Data
                formContext.getControl("new_authorizedsubdata1").setDisabled(true);
                formContext.getControl("new_authorizedsubdata2").setDisabled(true);

                //MasterFields, Disabled at form level.
            }  
        },

        isNullOrEmpty: function (s) {
            debugger;
            return (s === null || s === "");
        },

          GetFormattedDate: function (date) {
              debugger;
            var mm = String(date.getMinutes()).padStart(2, '0');
            var hh = String(date.getHours()).padStart(2, '0');
            var dd = String(date.getDate()).padStart(2, '0');
            var mm = String(date.getMonth() + 1).padStart(2, '0'); //January is 0!
            var yyyy = date.getFullYear();

            date = `${yyyy}/${mm}/${dd} ${hh}:${mm}`;

            return date;
        },

        setAuthorizedOnChange: function (executionContext) {
            debugger;
            var formContext = executionContext.getFormContext();
            if (formContext.getAttribute("new_authorize").getValue() === true && CrmJS.Kazang.SubEntity.Form.Functions.isNullOrEmpty(formContext.getAttribute("new_authorisedby").getValue())) {
                var nameAuthorized = formContext.context.userSettings.userName;
                var currDate = new Date();
                var formattedDate = CrmJS.Kazang.SubEntity.Form.Functions.GetFormattedDate(currDate);
                formContext.getAttribute("new_authorisedby").setValue(nameAuthorized + " - " + formattedDate);
            }
        },

        __namespace: true
      },
	  	  
      __namespace: true
    },

    Ribbon: {
      __namespace: true
    },

    __namespace: true
  }
}

var entityJS = CrmJS.Kazang.SubEntity;