Ext.define('App.controller.core.ImportExportLogic', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'combineddirectorypanel #loadimporttemplatecsvbutton': {
                    click: this.onLoadImportTemplateCSVButtonClick
                },
                'combineddirectorypanel #loadimporttemplatexlsxbutton': {
                    click: this.onLoadImportTemplateXLSXButtonClick
                },
                'combineddirectorypanel #exportcsvbutton': {
                    click: this.onExportCSVButtonClick
                },
                'combineddirectorypanel #exportxlsxbutton': {
                    click: this.onExportXLSXButtonClick
                },
                'combineddirectorypanel component[itemgroup=loadimportbutton]': {
                    click: this.onShowImportFormButtonClick
                },
                'combineddirectorypanel #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                },
                'uploadfilewindow #ok': {
                    click: this.onUploadFileOkButtonClick
                },
            }
        });
    },

    onShowImportFormButtonClick: function (button) {
        var grid = this.getGridByButton(button),
            panel = grid.up('combineddirectorypanel'),
            viewClassName = App.Util.buildViewClassName(panel, panel.getBaseModel(), 'Import', 'ParamForm'),
            defaultResource = this.getDefaultResource(button),
            resource = Ext.String.format(button.resource || defaultResource, defaultResource),
            action = Ext.String.format(button.action, resource);

        var editor = Ext.create('App.view.core.common.UploadFileWindow', {
            title: l10n.ns('core').value('uploadFileWindowTitle'),
            parentGrid: grid,
            resource: resource,
            action: action
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

    onLoadImportTemplateCSVButtonClick: function (button) {
        var me = this;
        var grid = me.getGridByButton(button);
        var panel = grid.up('combineddirectorypanel');
        var store = grid.getStore();
        var proxy = store.getProxy();
        var actionName = button.action || 'DownloadTemplateCSV';
        var resource = button.resource || proxy.resourceName;
        panel.setLoading(true);

        var query = breeze.EntityQuery
            .from(resource)
            .withParameters({
                $actionName: actionName,
                $method: 'POST'
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

    onLoadImportTemplateXLSXButtonClick: function (button) {
        var me = this;
        var grid = me.getGridByButton(button);
        var panel = grid.up('combineddirectorypanel');
        var store = grid.getStore();
        var proxy = store.getProxy();
        var actionName = button.action || 'DownloadTemplateXLSX';
        var resource = button.resource || proxy.resourceName;
        panel.setLoading(true);

        var query = breeze.EntityQuery
            .from(resource)
            .withParameters({
                $actionName: actionName,
                $method: 'POST'
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

    onExportCSVButtonClick: function (button) {
        var me = this;
        var grid = me.getGridByButton(button);
        var panel = grid.up('combineddirectorypanel');
        var store = grid.getStore();
        var proxy = store.getProxy();
        var actionName = button.action || 'ExportCSV';
        var resource = button.resource || proxy.resourceName;
        panel.setLoading(true);

        var query = breeze.EntityQuery
            .from(resource)
            .withParameters({
                $actionName: actionName,
                $method: 'POST'
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

    onExportXLSXButtonClick: function (button) {
        var me = this;
        var grid = me.getGridByButton(button);
        var panel = grid.up('combineddirectorypanel');
        var store = grid.getStore();
        var proxy = store.getProxy();
        var actionName = button.action || 'ExportXLSX';
        var resource = button.resource || proxy.resourceName;
        panel.setLoading(true);

        var query = breeze.EntityQuery
            .from(resource)
            .withParameters({
                $actionName: actionName,
                $method: 'POST'
            });

        query = me.buildQuery(query, store)
            .using(Ext.ux.data.BreezeEntityManager.getEntityManager())
            .execute()
            .then(function (data) {
                panel.setLoading(false);
                App.Notify.pushInfo('Export task created successfully');
                App.System.openUserTasksPanel()
            })
            .fail(function (data) {
                panel.setLoading(false);
                App.Notify.pushError(me.getErrorMessage(data));
            });
    },

    getErrorMessage: function (data) {
        var result = 'Unknown error';
        if (data && data.msg) {
            result = data.msg;
        } else if (data && data.message) {
            result = data.msg;
        } else if (data && data.body) {
            if (data.body["odata.error"]) {
                result = data.body["odata.error"].innererror.message;
            } else if (data.body.value) {
                result = data.body.value;
            }
        }
        return result;
    },

    onApplyImportButtonClick: function (button) {
        var me = this;
        var grid = this.getGridByButton(button);
        var cdPanel = grid.up('combineddirectorypanel');
        var resourceName = this.getResourceName(grid, '');
        var url = Ext.String.format('odata/{0}/Apply', resourceName); // Формирование URL получения ИД текущего импорта
        if (grid.importData) {
            cdPanel.setLoading(true);
            var importId = breeze.DataType.Guid.fmtOData(grid.importData.importId);
            var parameters = {
                $actionName: 'Apply',
                $method: 'POST',
                importId: importId
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

    buildQuery: function (query, store) {
        var proxy = store.getProxy();
        var extendedFilters = store.getExtendedFilter().getFilter();
        var operation = new Ext.data.Operation({
            action: 'read',
            filters: store.filters.items,
            fixedFilters:store.fixedFilters,
            extendedFilters: extendedFilters,
            sorters: store.sorters.items,
            groupers: store.groupers.items,
            pageMapGeneration: store.data.pageMapGeneration
        });
        query = proxy.applyExpand(query);
        query = proxy.applyFilter(operation, query);
        query = proxy.applyFixedFilter(operation, query);        
        query = proxy.applyExtendedFilter(operation, query);
        query = proxy.applySorting(operation, query);
        return query;
    },

    downloadFile: function (actionName, paramName, fileName) {
        var href = Ext.String.format('api/File/{0}?{1}={2}', actionName, paramName, fileName);
        var aLink = document.createElement('a');
        aLink.download = fileName;
        aLink.href = href;
        document.body.appendChild(aLink);
        aLink.click();
        document.body.removeChild(aLink)
    },

    onUploadFileOkButtonClick: function (button) {
        var me = this;
        var win = button.up('uploadfilewindow');
        var url = Ext.String.format("/odata/{0}/{1}", win.resource, win.action);
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
        if (form.isValid() && !isEmpty) {
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
    }
});