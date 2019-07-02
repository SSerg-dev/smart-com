Ext.define('App.controller.core.loophandler.UserLoopHandler', {
    extend: 'App.controller.core.CombinedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                //'userloophandler[isSearch!=true] directorygrid': {
                //    load: this.onGridStoreLoad
                //},
                'userloophandler directorygrid': {
                    // TODO: относиться к переключению состояний грида, можно убрать в будущем
                    itemdblclick: this.switchToDetailForm,

                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },

                // TODO: относиться к переключению состояний грида, можно убрать в будущем
                'userloophandler #datatable': {
                    activate: this.onActivateCard
                },
                'userloophandler #detailform': {
                    activate: this.onActivateCard
                },                
                'userloophandler #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'userloophandler #detailform #next': {
                    click: this.onNextButtonClick
                },

                'userloophandler #detail': {
                    click: this.switchToDetailForm
                },
                'userloophandler #table': {
                    click: this.onTableButtonClick
                },
                'userloophandler #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'userloophandler #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'userloophandler #createbutton': {
                    click: this.onCreateButtonClick
                },
                'userloophandler #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'userloophandler #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'userloophandler #historybutton': {
                    click: this.onHistoryButtonClick
                },

                'userloophandler #refresh': {
                    click: this.onRefreshButtonClick
                },
                'userloophandler #close': {
                    click: this.onCloseButtonClick
                },

                // import/export
                'userloophandler #exportbutton': {
                    click: this.onExportButtonClick
                },
                'userloophandler #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'userloophandler #loadimporttemplatecsvbutton': {
                    click: this.onLoadImportTemplateCSVButtonClick
                },
                'userloophandler #loadimporttemplatexlsxbutton': {
                    click: this.onLoadImportTemplateXLSXButtonClick
                },
                'userloophandler #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                },

                'userloophandler': {
                    afterrender: this.onUserLoopHandlerGridRendered
                },

                'userloophandler #applyReportFilter': {
                    click: this.applyFilterInternal
                },

                'userloophandler #applyTaskFilter': {
                    click: this.applyFilterInternal
                },

                'userloophandler #clearTaskFilter': {
                    click: this.applyFilterInternal
                },
                'userloophandler #download': {
                    click: this.onDownloadResultButtonClick
                },

            }
        });
    },
    onDownloadResultButtonClick: function (button) {
        var me = this,
           grid = me.getGridByButton(button),
           selModel = grid.getSelectionModel(),
           resource = button.resource || 'LoopHandlers',
           action = button.action || 'Parameters';

        if (selModel.hasSelection()) {
            grid.setLoading(true);
                record = selModel.getSelection()[0],


            breeze.EntityQuery
                .from(resource)
                .withParameters({
                    $actionName: action,
                    $method: 'POST',
                    $entity: record.getProxy().getBreezeEntityByRecord(record)
                })
                .using(Ext.ux.data.BreezeEntityManager.getEntityManager())
                .execute()
                .then(function (data) {
                    var resultData = data.httpResponse.data.value;
                    var result = JSON.parse(resultData);

                    var filePresent = result.OutcomingParameters != null &&
                                      result.OutcomingParameters.FileModel != null &&
                                      result.OutcomingParameters.FileModel.Value != null &&
                                      result.OutcomingParameters.FileModel.Value.Name != null;
                    if (filePresent) {
                        var href = document.location.href + Ext.String.format('/api/File/{0}?{1}={2}', 'ExportDownload', 'filename', result.OutcomingParameters.FileModel.Value.Name);
                        var aLink = document.createElement('a');
                        aLink.download = "FileData";
                        aLink.href = href;
                        document.body.appendChild(aLink);
                        aLink.click();
                        document.body.removeChild(aLink);
                    } else {
                        App.Notify.pushInfo('Нет файла для скачивания');
                    }
                    grid.setLoading(false);
                })
                .fail(function (data) {
                    grid.setLoading(false);
                    App.Notify.pushError(me.getErrorMessage(data));
                });
        } else {
            console.log('No selection');
            App.Notify.pushInfo('Сначала нужно выделить задачу');
        }
               
    },
    onGridAfterrender: function (grid) {
        grid.getStore().on({
            scope: this,
            load: this.onGridStoreLoad,
            grid: grid
        });

        this.callParent(arguments);
    },

    onGridStoreLoad: function (store, records, successful, eOpts) {
        if (eOpts.grid) {
            this.initSelection(eOpts.grid, records);
        }
    },

    onUserLoopHandlerGridRendered: function (panel) {
        var intervalSeconds = 10;
        var grid = panel.down('directorygrid');
        setInterval(function (grid) {
            if (grid.isVisible(true)) {
                var store = grid.getStore();
                grid.setLoadMaskDisabled(true);
                store.load({
                    callback: function () {
                        grid.setLoadMaskDisabled(false);
                    }
                });
            }
        }, intervalSeconds * 1000, grid);
    },

    applyFilterInternal: function (button) {
        var filterIds = ['clearTaskFilter', 'applyTaskFilter', 'applyReportFilter'];
        var buttonId = button.getItemId();
        //debugger;
        button.setUI('blue-pressed-button-toolbar-toolbar');
        for (var key in filterIds) {
            if (filterIds[key] != buttonId) {
                var otherButton = button.up().down("#" + filterIds[key]);
                otherButton.setUI('gray-button-toolbar-toolbar');
            }
        }

        var grid = this.getGridByButton(button);
        var me = this;
        //Имя хендлера который будет обрабатывать отчеты
        //Необходимо использовать операции равно, неравно для тройного переключателя,
        //тк в Breeze нет простой возможности сделать фильтр с условием "Не содержит".
        //TODO пока указан хендлер обрабатывающий обычный CSV экспорт
        var filterLoopHandlerName = 'ProcessingHost.Handlers.Export.BaseExportHandler';

        var filterOperation = buttonId == 'applyTaskFilter' ? 'Equals' : 'NotEqual';
        //фильтруем по имени хендлера
        if (buttonId !== "clearTaskFilter")
            grid.getStore().setFixedFilter('ReportFilter', {
                property: 'Name',
                operation: filterOperation,
                value: filterLoopHandlerName
            });
        else
            grid.getStore().clearFixedFilters();

    }
});