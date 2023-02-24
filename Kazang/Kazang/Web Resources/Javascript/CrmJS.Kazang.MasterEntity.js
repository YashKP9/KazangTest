if (typeof CrmJS === "undefined") { CrmJS = { __namespace: true } }
if (typeof CrmJS.Kazang === "undefined") { CrmJS.Kazang = { __namespace: true } }
if (typeof CrmJS.Kazang.MasterEntity === "undefined") {
	CrmJS.Kazang.MasterEntity = {
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

        onSave: function (context) {
        },

        attachEvents: function () {
          
        },
		
        businessRules: function (executionContext, onLoad) { 
            debugger;
            var formContext = executionContext.getFormContext();

            if (formContext.ui.getFormType() !== 1) {
                //General
                formContext.getControl("new_name").setDisabled(true);
                formContext.getControl("createdon").setDisabled(true);
                formContext.getControl("ownerid").setDisabled(true);
                formContext.getControl("statecode").setDisabled(true);
                formContext.getControl("statuscode").setDisabled(true);
                formContext.getControl("new_lastupdated").setDisabled(true);

                //MasterFields
                formContext.getControl("new_bool1").setDisabled(true);
                formContext.getControl("new_date1").setDisabled(true);
                formContext.getControl("new_goptionset1").setDisabled(true);
                formContext.getControl("new_int1").setDisabled(true);
                formContext.getControl("new_moptionset1").setDisabled(true);
                formContext.getControl("new_optionset2").setDisabled(true);
                formContext.getControl("new_string1").setDisabled(true);
            }
			
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

var entityJS = CrmJS.Kazang.MasterEntity;