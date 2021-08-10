Ext.define('App.controller.core.rpa.RPA', {
    extend: 'App.controller.core.CombinedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'rpa[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.switchToDetailForm,
                },
                'rpa directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'rpa #datatable': {
                    activate: this.onActivateCard
                },
                'rpa #detailform': {
                    activate: this.onActivateCard
                },
                'rpa #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'rpa #detailform #next': {
                    click: this.onNextButtonClick
                },
                'rpa #detail': {
                    click: this.onDetailButtonClick
                },
                'rpa #table': {
                    click: this.onTableButtonClick
                },
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
                },
                // import/export
                'rpa #exportbutton': {
                    click: this.onExportButtonClick
                },
                'rpa #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'rpa #loadimporttemplatecsvbutton': {
                    click: this.onLoadImportTemplateCSVButtonClick
                },
                'rpa #loadimporttemplatexlsxbutton': {
                    click: this.onLoadImportTemplateXLSXButtonClick
                },
                'rpa #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                }
            }
        });
    },
    onCreateButtonClick: function(button) {
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
        var url = Ext.String.format("/odata/{0}/{1}", 'RPAs', 'UploadFile');
        
        if(rpaForm.isValid()){
            editor.setLoading(l10n.ns('core').value('savingText'));
            rpaForm.getForm().submit({
                url: url,
                waitMsg: l10n.ns('core').value('uploadingFileWaitMessageText'),
                success: function (fp, o) {                    
                    // Проверить ответ от сервера на наличие ошибки и отобразить ее, в случае необходимости
                    if (o.result) {
                        var pattern = '/odata/RPas/DownloadFile?fileName={0}';
                        var downloadFileUrl = document.location.href + Ext.String.format(pattern, o.result.fileName);
                        var URL = '<a href=' + downloadFileUrl + '>' + o.result.fileName + '</a>'
                        var infoText = 'Задача обработки импортируемого файла успешно создана';
                        App.Notify.pushInfo(infoText);
                        // Открыть панель задач
                        App.System.openUserTasksPanel();
                        
                        var model = editor.rpaModel ? editor.rpaModel : Ext.create('App.model.core.rpa.RPA');
                        model.editing = true;
                        var handlerName = rpaForm.down('combobox[name=HandlerName]').getValue();
                        var params = rpaForm.down('#params');
                        var parametr = params.items.items.map(function (el) {
                            return el.value;
                        }).join(';');
                        var constrains = Object.keys(App.UserInfo.getConstrains()).join(';');                        
                        model.set('HandlerName', handlerName);
                        model.set("Constraint", constrains);
                        model.set('Parametr', parametr);
                        model.set('Status', 'In Progress');
                        model.set('FileURL', URL);
                        model.set('LogURL','<a href=' + document.location.href +'>Test log URL</a>');
                        model.save({
                            scope: me,
                            success: function (record, operation) {
                                if (callback) {
                                    callback(true);
                                }
                                editor.setLoading(false);
                                editor.close();
                                grid.getStore().load();
                            },
                            failure: function () {
                                editor.setLoading(false);
                            }
                        });
                    } else {
                        App.Notify.pushError(o.result.message);
                    }
                },
                failure: function (fp, o) {
                    App.Notify.pushError(o.result.message || 'Ошибка при обработке запроса');
                }
            });
        }
    }
});