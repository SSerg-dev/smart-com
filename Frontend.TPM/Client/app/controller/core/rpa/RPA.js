Ext.define('App.controller.core.rpa.RPA', {
    extend: 'App.controller.core.CombinedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'rpa[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad
                },
                'rpa directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'rpa #datatable': {
                    activate: this.onActivateCard
                },
                /* 'rpa #table': {
                    click: this.onTableButtonClick
                }, */
                'rpa #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'rpa #createbutton': {
                    click: this.onCreateButtonClick
                },               
                'rpa #refresh': {
                    click: this.onRefreshButtonClick
                },
                'rpa #close': {
                    click: this.onCloseButtonClick
                },
                //RPA Form
                'rpaformtoolbar #saveRPAForm': {
                    click: this.onSaveRPAFormClick
                },
                'rpaformtoolbar #cancelRPAForm': {
                    click: this.onCancelRPAFormClick
                }
            }
        });
    },

    onCreateButtonClick: function (button) {
       var editor = Ext.widget('customrpaeditor');
       editor.down('#params').setVisible(false);
       editor.show();
    },

    onCancelRPAFormClick: function(button) {
        var editor = button.up('customrpaeditor');
        editor.close();
    },

    onSaveRPAFormClick: function(button) {
       this.SaveRPA(button,null); 
    },

    SaveRPA: function(button, callback) {
        var me = this,
            editor = button.up('customrpaeditor'),
            grid = Ext.ComponentQuery.query('directorygrid')[1];
        setTimeout(function () {
           me.generateAndSendModel(editor, callback, me, grid), 
        0});
    },

    ValidateFields: function() {

    },
    generateAndSendModel: function(editor, callback, scope, grid) {
        var me = scope,
        rpaForm = editor.down('rpaform');        
        if(rpaForm.isValid()){
            editor.setLoading(l10n.ns('core').value('savingText'));
            var rpaModel = editor.rpaModel ? editor.rpaModel : Ext.create('App.model.core.rpa.RPA');
            var handlerName = rpaForm.down('combobox[name=HandlerName]').getValue();
            var userName = App.UserInfo.getUserName();
            var params = rpaForm.down('#params');
            var parametr = params.items.items.filter(el => el.value !== "").map((el) => el.value).join(';');
            var rpaType = rpaForm.getForm().findField('rpaType').getValue();
            rpaModel.set('HandlerName', handlerName);            
            rpaModel.set('UserName', userName);
            rpaModel.set('Parametrs', parametr);
            rpaModel.set('Status', 'Waiting');      
            var uploadFile = rpaForm.up().down('filefield').el.down('input[type=file]').dom.files[0];
            var formData = new FormData();
            formData.append('file', uploadFile);            
            var url = Ext.String.format("/odata/{0}/{1}", 'RPAs', 'SaveRPA');
            Ext.Ajax.request({   
                method: 'POST',   
                url: url,
                rawData: formData,   
                params: {'Model': Ext.JSON.encode(rpaModel.data), 'RPAType': rpaType},
                headers: {'Content-Type': null},
                success: function (data) {
                    App.Notify.pushInfo(Ext.JSON.decode(Ext.JSON.decode(data.responseText).value).message);
                    editor.setLoading(false);
                    editor.close();
                    grid.getStore().load();
                },
                fail: function (data) {
                    App.Notify.pushInfo(Ext.JSON.decode(Ext.JSON.decode(data.responseText).value).message);
                    editor.setLoading(false);
                    editor.close();
                    grid.getStore().load();
                }
            })
        }
    }
});