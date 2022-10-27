Ext.define('App.controller.tpm.promoproductsview.PromoProductsView', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'promoproductsview': {
                    afterrender: this.onAfterRender,
                },

                'promoproductsview[isMain=true][isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                },
                'promoproductsview[isSearch!=true] directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                },
                'promoproductsview directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'promoproductsview #datatable': {
                    activate: this.onActivateCard
                },
                'promoproductsview #detailform': {
                    activate: this.onActivateCard
                },
                'promoproductsview #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'promoproductsview #detailform #next': {
                    click: this.onNextButtonClick
                },
                'promoproductsview #detail': {
                    click: this.onDetailButtonClick
                },
                'promoproductsview #table': {
                    click: this.onTableButtonClick
                },
                'promoproductsview #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'promoproductsview #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'promoproductsview #createbutton': {
                    click: this.onCreateButtonClick
                },
                'promoproductsview #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'promoproductsview #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'promoproductsview #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'promoproductsview #refresh': {
                    click: this.onRefreshButtonClick
                },
                'promoproductsview #close': {
                    click: this.onCloseButtonClick
                },
                //// import/export
                'promoproductsview #customexportxlsxbutton': {
                    click: this.onExportButtonClick
                },
                'promoproductsview #customloadimporttemplatexlsxbutton': {
                    click: this.onDownloadTemplateXLSX
                },
                'promoproductsview component[itemgroup=customloadimportbutton]': {
                    click: this.onShowImportFormButtonClick
                },
                'promoproductsview #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'promoproductsview #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                },
                '#customuploadfilewindow #userOk': {
                    click: this.onUploadFileOkButtonClick
                },
            }
        });
    },

    onAfterRender: function (promoProductsView) {
        var currentRole = App.UserInfo.getCurrentRole()['SystemName'];
        promoProductsView.storePromoProductsView = Ext.create('Ext.data.Store', {
            model: 'App.model.tpm.promoproductcorrection.PromoProductCorrection',
            autoLoad: false,
            root: {}
        });

        // проверка точек доступа
        if (promoProductsView.crudAccess.length > 0 && promoProductsView.crudAccess.indexOf(currentRole) === -1) {
            promoProductsView.isReadable = true
        }

        // RSmode
        var settingStore = Ext.data.StoreManager.lookup('settingLocalStore');
        var mode = settingStore.findRecord('name', 'mode');
        var model = Ext.ComponentQuery.query('promoeditorcustom')[0].model;
        if (mode.data.value == 1 && model.data.TPMmode == 'Current') {
            promoProductsView.isReadable = true;
        }
        var sg = Ext.ComponentQuery.query('[itemId=GlyphLock]');
        // если грид открывается для чтения
        if (promoProductsView.isReadable || Ext.ComponentQuery.query('[itemId=GlyphLock]')[0].disabled) {
            promoProductsView.down('#updatebutton').setVisible(false);

            Ext.ComponentQuery.query('basewindow #ok').forEach(function (item) {
                item.setVisible(false);
            });

            promoProductsView.down('customheadermenu[itemId=importExport]').down('[itemgroup=customloadimportbutton]').setVisible(false);
            promoProductsView.down('customheadermenu[itemId=importExport]').down('[itemId=customloadimporttemplatexlsxbutton]').setVisible(false);
        };

    },

    onUpdateButtonClick: function (button) {
        var grid = button.up('promoproductsview').down('directorygrid');
        var selModel = grid.getSelectionModel();
        if (selModel.hasSelection()) {
            var selected = selModel.getSelection()[0];
            this.startEditRecord(selected, grid);
        }
    },

    onDetailButtonClick: function (button) {
        var grid = button.up('promoproductsview').down('directorygrid');
        selModel = grid.getSelectionModel();

        if (selModel.hasSelection()) {
            this.startDetailRecord(selModel.getSelection()[0], grid);
        } else {
            console.log('No selection');
        }
    },

    startDetailRecord: function (model, grid) {
        this.editor = grid.editorModel.createEditor({ title: l10n.ns('core').value('detailWindowTitle') });
        this.editor.model = model;
        this.editor.grid = grid;

        //Проверка на доступ к изменению данной модели для тоже скрытия
        var toEditAccess = false;
        if (model.proxy) {
            if (model.proxy.type == 'breeze') {
                toEditAccess = App.UserInfo.hasAccessPoint(model.proxy.resourceName, 'Patch');
            }
        }

        this.editor.down('editorform').getForm().getFields().each(function (field, index, len) {
            field.setReadOnly(true);
        }, this);

        if (toEditAccess && !grid.up('promoproductsview').isReadable) {
            this.editor.down('#ok').setVisible(false);
            this.editor.down('#canceledit').setVisible(false);

            this.editor.down('#ok').on('click', this.onOkButtonClick, this);
            this.editor.down('#edit').on('click', this.onEditButtonClick, this);
            this.editor.down('#canceledit').on('click', this.onCancelButtonClick, this);
            this.editor.down('#close').on('click', this.onCloseButtonClick, this);
            this.editor.on('close', this.onEditorClose, this);

        } else {
            this.editor.down('#ok').setVisible(false);
            this.editor.down('#edit').setVisible(false);
            this.editor.down('#canceledit').setVisible(false);

            this.editor.down('#close').on('click', this.onCloseButtonClick, this);
            this.editor.on('close', this.onEditorClose, this);
        }

        this.editor.down('editorform').loadRecord(model);
        this.editor.show();
    },

    startEditRecord: function (model, grid) {
        this.editor = grid.editorModel.createEditor({ title: l10n.ns('core').value('updateWindowTitle') });
        this.editor.grid = grid;
        if (this.editor.down('#edit') && this.editor.down('#close')) {
            this.editor.down('#edit').setVisible(false);
            this.editor.down('#close').setVisible(false);
        }

        this.editor.down('#ok').on('click', this.onOkButtonClick, this);
        if (this.editor.down('#canceledit'))
            this.editor.down('#canceledit').on('click', this.onCancelButtonClick, this);
        else
            this.editor.down('#close').on('click', this.onCloseButtonClick, this);
        this.editor.on('close', this.onEditorClose, this);

        this.editor.down('editorform').loadRecord(model);
        this.editor.show();

        this.editor.afterWindowShow(this.editor, false);
        this.editor.down('editorform').getForm().getFields().each(function (field, index, len) {
            if (field.xtype === 'singlelinedisplayfield')
                field.setReadOnly(false);
        }, this);
    },

    onEditButtonClick: function (button) {
        this.editor.down('#ok').setVisible(true);
        this.editor.down('#canceledit').setVisible(true);
        this.editor.down('#edit').setVisible(false);
        this.editor.down('#close').setVisible(false);
        this.editor.setTitle(l10n.ns('core').value('updateWindowTitle'));
        this.detailMode = true;

        this.editor.afterWindowShow(this.editor, false);
        this.editor.down('editorform').getForm().getFields().each(function (field, index, len) {
            field.setReadOnly(false);
        }, this);
    },

    onOkButtonClick: function (button) {
        var form = this.editor.down('editorform').getForm(),
            record = form.getRecord(),
            oldUpliftPercent = record.data.PlanProductUpliftPercent;

        // RSmode
        var settingStore = Ext.data.StoreManager.lookup('settingLocalStore');
        var mode = settingStore.findRecord('name', 'mode');

        this.editor.setLoading(l10n.ns('core').value('savingText'));

        if (!form.isValid()) {
            return;
        }

        var errors = record.validate();

        if (!errors.isValid()) {
            form.markInvalid(errors);
            return;
        }

        form.updateRecord();

        var newUpliftPercent = record.data.PlanProductUpliftPercent;

        if (oldUpliftPercent !== newUpliftPercent) {

            var _promoProductCorrection = new App.model.tpm.promoproductcorrection.PromoProductCorrection({
                PromoProductId: record.data.Id,
                PlanProductUpliftPercentCorrected: newUpliftPercent,
                TempId: Ext.ComponentQuery.query('promoeditorcustom')[0].tempEditUpliftId,
                TPMmode: mode.data.value,
            });
            Ext.ComponentQuery.query('promoeditorcustom')[0].productUpliftChanged = true;
            this.createPromoProductCorrection(_promoProductCorrection);
        } else {
            this.editor.close();
        };
    },

    // функция для добавления Изменения объекта в гриде
    createPromoProductCorrection: function (model, callback) {
        var grid = this.editor.grid,
            store = grid.getStore(),
            promoProductsView = this.editor.grid.up('promoproductsview');

        var index = promoProductsView.storePromoProductsView.find('PromoProductId', model.data.PromoProductId);
        if (index > -1) {
            promoProductsView.storePromoProductsView.getAt(index).set('PlanProductUpliftPercentCorrected', model.data.PlanProductUpliftPercentCorrected);
            promoProductsView.storePromoProductsView.getAt(index).set('TempId', Ext.ComponentQuery.query('promoeditorcustom')[0].tempEditUpliftId);
        } else {
            promoProductsView.storePromoProductsView.add(model);
        };

        this.editor.setLoading(false);
        this.editor.close();
    },

    onCancelButtonClick: function (button) {
        //если редактирование вызвано из режима просмотра, то при отмене редактирования происходит возврат на форму просмотра
        var form = this.editor.down('editorform').getForm(),
            record = form.getRecord();
        form.loadRecord(record);

        if (this.detailMode) {
            this.editor.down('#ok').setVisible(false);
            this.editor.down('#canceledit').setVisible(false);
            this.editor.down('#edit').setVisible(true);
            this.editor.down('#close').setVisible(true);
            this.editor.setTitle(l10n.ns('core').value('detailWindowTitle'));
            this.detailMode = false;

            this.editor.down('editorform').getForm().getFields().each(function (field, index, len) {
                field.setReadOnly(true);
                field.focus(false);
            }, this);
        } else {
            this.editor.close();
        }
    },

    onCloseButtonClick: function (button) {
        //this.editor.grid.getStore().load();
        // Window will close automatically.
    },

    onEditorClose: function (window) {
        var form = this.editor.down('editorform'),
            record = form.getRecord();

        if (record) {
            //record.reject();
        }

        form.getForm().reset(true);
        this.editor = null;
        this.detailMode = null;
    },
    onDownloadTemplateXLSX: function (button) {
        console.log("asd");
        var me = this;
        var grid = me.getGridByButton(button);
        var panel = grid.up('combineddirectorypanel');
        var store = grid.getStore();
        var proxy = store.getProxy();
        var actionName = button.action || 'DownloadTemplateXLSX';
        var resource = button.resource || proxy.resourceName;
        var promoId = breeze.DataType.Guid.fmtOData(button.up('promoproductsview').promoId);

        panel.setLoading(true);

        var query = breeze.EntityQuery
            .from(resource)
            .withParameters({
                $actionName: actionName,
                $method: 'POST',
                promoId: promoId,
            });

        query = me.buildQuery(query, store)
            .using(Ext.ux.data.BreezeEntityManager.getEntityManager())
            .execute()
            .then(function (data) {
                panel.setLoading(false);
                var filename = data.httpResponse.data.value;
                me.downloadFile('ExportDownload', 'filename', filename);
            })
            .fail(function (data) {
                panel.setLoading(false);
                App.Notify.pushError(me.getErrorMessage(data));
            });
    },
    onExportButtonClick: function (button) {
        var me = this;
        var grid = me.getGridByButton(button);
        var panel = grid.up('combineddirectorypanel');
        var store = grid.getStore();
        var proxy = store.getProxy();
        var actionName = button.action || 'ExportXLSX';
        var resource = button.resource || proxy.resourceName;
        var promoId = breeze.DataType.Guid.fmtOData(button.up('promoproductsview').promoId);

        panel.setLoading(true);

        var query = breeze.EntityQuery
            .from(resource)
            .withParameters({
                $actionName: actionName,
                $method: 'POST',
                promoId: promoId,
            });

        query = me.buildQuery(query, store)
            .using(Ext.ux.data.BreezeEntityManager.getEntityManager())
            .execute()
            .then(function (data) {
                $.connection.sessionHub.server.startMoniringHandler(data.httpResponse.data.value)
                    .done(function () {
                        panel.setLoading(false);
                    })
                    .fail(function (reason) {
                        console.log("SignalR connection failed: " + reason);
                        panel.setLoading(false);
                    });
            })
            .fail(function (data) {
                panel.setLoading(false);
                App.Notify.pushError(me.getErrorMessage(data));
            });
    },

    onShowImportFormButtonClick: function (button) {
        var grid = this.getGridByButton(button),
            panel = grid.up('combineddirectorypanel'),
            viewClassName = App.Util.buildViewClassName(panel, panel.getBaseModel(), 'Import', 'ParamForm'),
            defaultResource = this.getDefaultResource(button),
            resource = Ext.String.format(button.resource || defaultResource, defaultResource),
            action = Ext.String.format(button.action, resource),
            promoId = panel.promoId;

        var editor = Ext.create('App.view.core.common.UploadFileWindow', {
            title: l10n.ns('core').value('uploadFileWindowTitle'),
            itemId: 'customuploadfilewindow',
            parentGrid: grid,
            resource: resource,
            action: action,
            promoId: promoId,
            buttons: [{
                text: l10n.ns('core', 'buttons').value('cancel'),
                itemId: 'cancel'
            }, {
                text: l10n.ns('core', 'buttons').value('upload'),
                ui: 'green-button-footer-toolbar',
                itemId: 'userOk'
            }]
        });

        if (button.additionParameters) {
            var fields = [];
            for (var param in button.additionParameters) {
                if (button.hasOwnProperty(param)) {
                    fields.push({
                        xtype: 'hiddenfield',
                        name: param,
                        value: button.additionParameters[param]
                    });
                }
            }
            editor.down('editorform').add(fields);
        }
        var btnBrowse = editor.down('filefield');
        if (btnBrowse) {
            var allowFormat = button.allowFormat || ['csv', 'zip'];
            btnBrowse.allowFormat = allowFormat;
            btnBrowse.vtypeText = 'Формат файла не поддерживается. Необходим файл формата: ' + allowFormat.join(',');
        }

        if (Ext.ClassManager.get(viewClassName)) {
            var paramForm = Ext.create(viewClassName);
            var fieldValues = button.fieldValues ? Ext.clone(button.fieldValues) : null;
            paramForm.initFields(fieldValues);
            editor.down('#importform').insert(0, paramForm);
        }

        editor.show();
    },

    onApplyImportButtonClick: function (button) {
        var me = this;
        var grid = this.getGridByButton(button);
        var cdPanel = grid.up('combineddirectorypanel');
        var resourceName = this.getResourceName(grid, '');
        var promoId = breeze.DataType.Guid.fmtOData(button.up('promoproductsview').promoId);
        var url = Ext.String.format('odata/{0}/Apply', resourceName); // Формирование URL получения ИД текущего импорта
        if (grid.importData) {
            cdPanel.setLoading(true);
            var importId = breeze.DataType.Guid.fmtOData(grid.importData.importId);
            var parameters = {
                $actionName: 'Apply',
                $method: 'POST',
                importId: importId,
                promoId: promoId
            };
            function upcapitalizeFirstLetter(string) {
                return string.charAt(0).toLowerCase() + string.slice(1);
            };
            Ext.Object.each(grid.importData.crossParams, function (key, value) {
                var paramName = upcapitalizeFirstLetter(key.replace('CrossParam.', ''));
                parameters[paramName] = value;
            }, this);

            breeze.EntityQuery
                .from(resourceName)
                .withParameters(parameters)
                .using(Ext.ux.data.BreezeEntityManager.getEntityManager())
                .execute()
                .then(function (data) {
                    cdPanel.setLoading(false);
                    var result = Ext.JSON.decode(data.httpResponse.data.value);
                    if (result.success) {
                        App.Notify.pushInfo('Задача обработки импорта успешно создана');
                        // Закрыть окно импорта и параметров отложенной задачи
                        var win = grid.up('window');
                        var parentWindow;
                        var panel = win.down('combineddirectorypanel');
                        if (panel) {
                            parentWindow = panel.parentWindow;
                        }
                        if (win) {
                            win.close();
                        }
                        if (parentWindow && parentWindow.close) {
                            parentWindow.close();
                        }
                        // Открыть панель задач
                        App.System.openUserTasksPanel()
                    } else {
                        App.Notify.pushError(result.message);
                    }


                })
                .fail(function (data) {
                    cdPanel.setLoading(false);
                    App.Notify.pushError(me.getErrorMessage(data));
                });
        } else {
            console.log('Нет информации об импорте')
        }
    },

    onUploadFileOkButtonClick: function (button) {
        var me = this;
        //rsmode
        var settingStore = Ext.data.StoreManager.lookup('settingLocalStore');
        var mode = settingStore.findRecord('name', 'mode');
        var win = button.up('uploadfilewindow');
        var promoId = breeze.DataType.Guid.fmtOData(Ext.ComponentQuery.query('promoeditorcustom')[0].promoId);
        var url = Ext.String.format("/odata/{0}/{1}?promoId={2}&tempEditUpliftId={3}&tPMmode={4}", win.resource, win.action, promoId, Ext.ComponentQuery.query('promoeditorcustom')[0].tempEditUpliftId, mode.data.value);
        var needCloseParentAfterUpload = win.needCloseParentAfterUpload;
        var parentWin = win.parentGrid ? win.parentGrid.up('window') : null;
        var form = win.down('#importform');
        var paramform = form.down('importparamform');
        var isEmpty;
        if (paramform) {
            var constrains = paramform.query('field[isConstrain=true]');
            isEmpty = constrains && constrains.length > 0 && constrains.every(function (item) {
                return Ext.isEmpty(item.getValue());
            });

            if (isEmpty) {
                paramform.addCls('error-import-form');
                paramform.down('#errormsg').getEl().setVisible();
            }
        }
        if (form.isValid() && !isEmpty)
        {
            form.getForm().baseParams = {
                promoId: promoId,
                tempEditUpliftId: Ext.ComponentQuery.query('promoeditorcustom')[0].tempEditUpliftId
            };

            form.getForm().submit({
                url: url,
                waitMsg: l10n.ns('core').value('uploadingFileWaitMessageText'),
                success: function (fp, o) {
                    // Проверить ответ от сервера на наличие ошибки и отобразить ее, в случае необходимости
                    if (o.result) {
                        win.close();
                        if (parentWin && needCloseParentAfterUpload) {
                            parentWin.close();
                        }
                        var infoText = win.successMessage || 'Задача обработки импортируемого файла успешно создана';
                        Ext.ComponentQuery.query('promoeditorcustom')[0].productUpliftChanged = true;
                        App.Notify.pushInfo(infoText);
                        // Открыть панель задач
                        if (!win.isNotTask) {
                            App.System.openUserTasksPanel();
                        }
                    } else {
                        App.Notify.pushError(o.result.message);
                    }
                },
                failure: function (fp, o) {
                    App.Notify.pushError(o.result.message || 'Ошибка при обработке запроса');
                }
            });
        }
    },
});