Ext.define('App.controller.tpm.promolinked.PromoLinked', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'promolinkedticosts directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'promolinkedticosts #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'promolinkedticosts #refresh': {
                    click: this.onRefreshButtonClick
                },
                'promolinkedticosts #close': {
                    click: this.onCloseButtonClick
                },
                // import/export
                'promolinkedticosts #customexportxlsxbutton': {
                    click: this.onExportTICostsBtnClick
                },
                'promolinkedticosts #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'promolinkedticosts #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'promolinkedticosts #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                },                
                'promolinkedticosts #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'promolinkedticosts #addbutton': {
                    click: this.onAddButtonClick
                },
                ///
                'promolinkedcostprod directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'promolinkedcostprod #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'promolinkedcostprod #refresh': {
                    click: this.onRefreshButtonClick
                },
                'promolinkedcostprod #close': {
                    click: this.onCloseButtonClick
                },
                // import/export
                'promolinkedcostprod #exportbutton': {
                    click: this.onExportButtonClick
                },                                           
                ///
                'promolinkedviewer': {
                    afterrender: this.onPromoLinkedViewerAfterRender
                },
                'promolinkedviewer #addbutton': {
                    click: this.onAddViewerButtonClick
                },
                'promolinkedviewer #updatebutton': {
                    click: this.onUpdateViewerButtonClick
                },
                'promolinkedviewer #deletebutton': {
                    click: this.onPromoLinkedViewerDeleteButtonClick
                },
                'promolinkedviewer #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                '#associatedpromosupport_promo_selectorwindow directorygrid': {
                    selectionchange: this.onSelectorGridSelectionChange
                },
                '#associatedpromosupport_promo_selectorwindow #select': {
                    click: this.onSelectButtonClick
                },
                '#associatedpromosupport_promo_selectorwindow promo': {
                    afterrender: this.promoAfterRender
                },

                'basewindow[name=choosepromowindow] #apply': {
                    click: this.onApplyActionButtonClick
                },

                'choosepromo directorygrid': {
                    selectionchange: this.onChoosePromoGridCheckChange,                    
                },                              
                'choosepromo #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'choosepromo gridcolumn[cls=select-all-header]': {
                    headerclick: this.onSelectAllRecordsClick,
                    afterrender: this.clearBaseSelectAllRecordsHandler,
                },

                'pspcosttieditor #ok': {
                    click: this.onOkpspcosttieditorClick,
                },
                'pspcostprodeditor #ok': {
                    click: this.onOkpspcostprodeditorClick,
                }
            }
        });
    },

    onAddButtonClick: function (button) {
        var promoLinked = button.up('promolinkedticosts'),
            promoSupport = promoLinked.up().down('promosupport') || promoLinked.up().down('costproduction'),
            promoSupportGrid = promoSupport.down('grid'),
            selModel = promoSupportGrid.getSelectionModel();

        if (selModel.hasSelection()) {
            var selected = selModel.getSelection()[0];
            this.showChoosePromoWindow(selected.data.Id, promoSupportGrid, promoLinked.down('grid'));
        }        
    },

    onAddViewerButtonClick: function (button) {
        var customPromoSupportEditor = button.up('custompromosupporteditor');
        var startDateField = customPromoSupportEditor.down('[name=StartDate]');
        var endDateField = customPromoSupportEditor.down('[name=EndDate]');

        if (startDateField && endDateField && startDateField.getValue() !== null && endDateField.getValue() !== null) {
            var promoLinkedViewer = button.up('promolinkedviewer'),
                promoLinkedGrid = promoLinkedViewer.down('grid');

            this.showChoosePromoWindow(null, null, promoLinkedGrid);
        }
        else {
            App.Notify.pushInfo(l10n.ns('tpm', 'PromoSupportPromo').value('DatesNotChoosen'));
            startDateField.validate();
            endDateField.validate();            
        }
    },

    showChoosePromoWindow: function (promoSupportId, promoSupportGrid, promoLinkedGrid) {
        var choosepromowindow = Ext.create('App.view.core.base.BaseModalWindow', {
            title: 'Choose Promo',
            name: 'choosepromowindow',
            width: 950,
            height: 650,
            minWidth: 950,
            minHeight: 650,
            promoSupportId: promoSupportId,
            promoSupportGrid: promoSupportGrid,
            promoLinkedGrid: promoLinkedGrid,
            layout: 'fit',
            items: [{
                xtype: 'choosepromo'
            }],
            buttons: [{
                text: l10n.ns('core', 'buttons').value('cancel'),
                itemId: 'cancel'
            }, {
                text: l10n.ns('core', 'buttons').value('ok'),
                ui: 'green-button-footer-toolbar',
                itemId: 'apply',
                disabled: true
            }]
        });

        var choosePromoWindowGrid = choosepromowindow.down('grid');

        //установака галочек на уже выбранные промо при открытии грида в окне создания/редактирования Promo Support
        if (promoLinkedGrid) {
            var promoLinkedStore = promoLinkedGrid.getStore(),
                count = promoLinkedStore.getCount(),
                promoLinkedRecords = count > 0 ? promoLinkedStore.getRange(0, count) : [],
                choosePromoStore = choosePromoWindowGrid.getStore();         

            // скрываем уже выбранные промо
            choosePromoStore.addListener('load', function () {
                var viewChoosePromoGrid = choosePromoWindowGrid.getView();

                promoLinkedRecords.forEach(function (checkedRow) {
                    var recordInChoosePromo = choosePromoStore.findRecord('Id', checkedRow.data.PromoId);
                    if (recordInChoosePromo)
                        viewChoosePromoGrid.addRowCls(recordInChoosePromo.index, 'hidden-row-grid');
                });
            });
            choosepromowindow.show();
        }
          
        var editor = promoLinkedGrid.up('custompromosupporteditor');
        var prefilter = {
            operator: "and",
            rules: [{
                operation: "NotEqual",
                property: "PromoStatus.Name",
                value: "Deleted"
            }, {
                operation: "NotEqual",
                property: "PromoStatus.Name",
                value: "Closed"
            }, {
                operation: "NotEqual",
                property: "PromoStatus.Name",
                value: "Draft"
            }]
        };
        var client;
        var startDate;
        var endDate;

        // если создаем/редактируем запись
        if (editor) {
            client = editor.clientFullPathName ? editor.clientFullPathName : editor.promoSupportModel.get('ClientTreeFullPathName');
            startDate = editor.down('[name=StartDate]').getValue();
            endDate = editor.down('[name=EndDate]').getValue();
        }
        else {
            // если прикрепляем через uhbl linked promo, то ищем через грид, в котором выбрана запись
            var costproductionGrid = Ext.ComponentQuery.query('costproduction grid');
            var promosupportGrid = Ext.ComponentQuery.query('promosupport grid');
            var psGrid = costproductionGrid.length > 0 ? costproductionGrid[0] : promosupportGrid[0];

            client = psGrid.getSelectionModel().getSelection()[0].get('ClientTreeFullPathName');
            startDate = psGrid.getSelectionModel().getSelection()[0].get('StartDate');
            endDate = psGrid.getSelectionModel().getSelection()[0].get('EndDate');
        }  

        startDate = changeTimeZone(startDate, 3, -1);
        endDate = changeTimeZone(endDate, 3, -1);

        prefilter.rules.push({
            operation: "Equals",
            property: "ClientHierarchy",
            value: client
        });

        if (startDate) {
            prefilter.rules.push({
                operation: "GraterOrEqual",
                property: "EndDate",
                value: startDate
            });
        }

        if (endDate) {
            prefilter.rules.push({
                operation: "LessOrEqual",
                property: "StartDate",
                value: endDate
            });
        }

        // устанавливаем префильтр
        choosePromoWindowGrid.getStore().setFixedFilter('PreFilter', prefilter);
    },

    onApplyActionButtonClick: function (button) {
        var window = button.up('basewindow[name=choosepromowindow]'),
            grid = window.down('grid'),
            checkedRows = grid.getSelectionModel().getCheckedRows(),
            promoIdString = '';        

        if (window.promoSupportId && window.promoSupportGrid) {
            //привязка нескольких промо к существующему PromoSupport (делается в мастер-детейл)
            checkedRows.forEach(function (item) {
                promoIdString += item.data.Id + ';';
            });

            parameters = {
                promoIdString: breeze.DataType.String.fmtOData(promoIdString),
                promoSupportId: breeze.DataType.Guid.fmtOData(window.promoSupportId)
            };

            var associated = window.promoSupportGrid.up('associatedpromosupport') || window.promoSupportGrid.up('associatedcostproduction');
            var promoLinkedGrid = associated.down('promolinkedticosts').down('grid') || associated.down('costproductionpromolinked').down('grid');;

            window.setLoading(l10n.ns('core').value('savingText'));
            App.Util.makeRequestWithCallback('PromoSupportPromoes', 'PromoSuportPromoPost', parameters, function (data) {
                var result = Ext.JSON.decode(data.httpResponse.data.value);
                if (result.success) {
                    promoLinkedGrid.getStore().on('load', function () {
                        window.setLoading(false);
                    });

                    promoLinkedGrid.getStore().load();
                    window.close();
                }
                else {
                    App.Notify.pushError(result.message);
                    window.setLoading(false);
                }
            });
        } else if (window.promoLinkedGrid) {
            //окно создания PromoSupport
            var promoLinkedStore = window.promoLinkedGrid.getStore();
            var promoLinkedProxy = promoLinkedStore.getProxy();            

            checkedRows.forEach(function (item) {
                var model = {
                    PromoId: item.data.Id,
                    Promo: item.raw
                };

                var promoSupportPromo = promoLinkedProxy.getReader().readRecords(model).records[0];

                promoLinkedProxy.data.push(promoSupportPromo);                
            });

            promoLinkedStore.load();
            window.close();
        }
    },

    promoAfterRender: function () {
        var toolbar = Ext.ComponentQuery.query('promo')[0].down('custombigtoolbar');

        toolbar.down('#createbutton').hide();
        toolbar.down('#updatebutton').hide();
        toolbar.down('#deletebutton').hide();
        toolbar.down('#historybutton').hide();
    },

    // блокировка/разблокировка кнопки Применить при изменении набора галочек в гриде привязки Promo к PromoSupport
    onChoosePromoGridCheckChange: function (model) {
        var grid = model.view.up('grid'),
            checkedRows = grid.getSelectionModel().getCheckedRows();

        if (checkedRows.length > 0) {
            grid.up('basewindow[name=choosepromowindow]').down('#apply').setDisabled(false);
        } else {
            grid.up('basewindow[name=choosepromowindow]').down('#apply').setDisabled(true);
        }
    },

    onPromoLinkedViewerDeleteButtonClick: function (button) {
        Ext.Msg.show({
            title: l10n.ns('tpm', 'PromoSupport').value('RemoveIntactWindowTitle'),
            msg: l10n.ns('tpm', 'PromoSupport').value('RemoveIntactConfirmMessage'),
            fn: onMsgBoxClose,
            scope: this,
            icon: Ext.Msg.QUESTION,
            buttons: Ext.Msg.YESNO,
            buttonText: {
                yes: l10n.ns('core', 'buttons').value('delete'),
                no: l10n.ns('core', 'buttons').value('cancel')
            }
        });

        function onMsgBoxClose(buttonId) {
            if (buttonId === 'yes') {
                var promoLinkedViewer = Ext.ComponentQuery.query('promolinkedviewer')[0];
                var selectedRecord = promoLinkedViewer.down('grid').getSelectionModel().getSelection()[0];
                var promoLinkedViewerStore = promoLinkedViewer.down('grid').getStore();
                var promoLinkedViewerProxy = promoLinkedViewerStore.getProxy();
                var mainPromoSupportRightToolbarContainer = Ext.ComponentQuery.query('#mainPromoSupportRightToolbarContainer')[0];
                
                promoLinkedViewerProxy.data = promoLinkedViewerProxy.data.filter(function (rec) { return rec.id != selectedRecord.id });
                promoLinkedViewerStore.load();
            }
        }
    },
    onPromoLinkedViewerAfterRender: function (panel) {
        var promoLinkedViewerGrid = panel.down('grid'); 
        var promoLinkedViewerStore = promoLinkedViewerGrid.getStore();

        promoLinkedViewerStore.addListener('load', function () {
            if (promoLinkedViewerStore.getCount() > 0) {
                promoLinkedViewerGrid.getSelectionModel().select(promoLinkedViewerStore.getAt(0));
            }
        });
    },

    clearBaseSelectAllRecordsHandler: function (header) {
        // избавляемся от некорректного обработчика
        var headerCt = header.up('headercontainer');

        if (headerCt.events.headerclick.listeners.length == 2) {
            headerCt.events.headerclick.listeners.pop();
        }
    },

    onSelectAllRecordsClick: function (headerCt, header) {
        var grid = header.up('directorygrid');
        var store = grid.getStore();
        var selModel = grid.getSelectionModel()
        var recordsCount = store.getCount();
        var functionChecker = selModel.checkedRows.length == recordsCount ? selModel.uncheckRows : selModel.checkRows;

        for (var i = 0; i < recordsCount; i++) {
            functionChecker.call(selModel, store.getAt(i));
        }

        grid.fireEvent('selectionchange', selModel);
    },

    // переопределение нажатия кнопки экспорта, для определения раздела (TI Cost/Cost Production)
    onExportTICostsBtnClick: function (button) {
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
                $method: 'POST',
                section: 'ticosts'
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

    onUpdateViewerButtonClick: function (button) {
        var promoLinkedViewer = button.up('promolinkedviewer');
        var promoLinkedGrid = promoLinkedViewer.down('grid');
        var supportEditorWindow = button.up('custompromosupporteditor');
        var selModel = promoLinkedGrid.getSelectionModel();
        var selected;

        if (selModel.hasSelection()) {
            selected = promoLinkedGrid.getSelectionModel().selected.items[0]; //selModel.getSelection()[0];
        }

        if (supportEditorWindow.costProduction) {
            var editor = Ext.create('App.view.tpm.promosupportpromo.PSPCostProdEditor', { pspId: selected.id });
            editor.down('[name=PlanCostProd]').setValue(selected.get('PlanCostProd'));
            editor.down('[name=FactCostProd]').setValue(selected.get('FactCostProd'));
            editor.show();
        }
        else {
            var editor = Ext.create('App.view.tpm.promosupportpromo.PSPCostTIEditor', { pspId: selected.id });
            editor.down('[name=PlanCalculation]').setValue(selected.get('PlanCalculation'));
            editor.down('[name=FactCalculation]').setValue(selected.get('FactCalculation'));
            
            if (supportEditorWindow.down('#promoSupportTypeField').getValue().toLowerCase().indexOf('posm') < 0)
                editor.down('[name=PlanCalculation]').setDisabled(true);

            editor.show();
        }
    },

    onOkpspcosttieditorClick: function (button) {  
        var pspcosttieditor = button.up('pspcosttieditor');
        var factCalculationField = pspcosttieditor.down('[name=FactCalculation]');
        var planCalculationField = pspcosttieditor.down('[name=PlanCalculation]');

        if (factCalculationField.isValid() && planCalculationField.isValid()) {
            var supportEditorWindow = Ext.ComponentQuery.query('custompromosupporteditor')[0];
            var promoLinkedViewer = supportEditorWindow.down('promolinkedviewer');
            var promoLinkedGrid = promoLinkedViewer.down('grid');
            var promoLinkedStore = promoLinkedGrid.getStore();
            var promoLinkedProxy = promoLinkedStore.getProxy();

            // ищем нужную запись
            for (var i = 0; i < promoLinkedProxy.data.length; i++) {
                if (promoLinkedProxy.data[i].id == pspcosttieditor.pspId) {
                    promoLinkedProxy.data[i].set('PlanCalculation', planCalculationField.getValue());
                    promoLinkedProxy.data[i].set('FactCalculation', factCalculationField.getValue());
                }
            }

            promoLinkedStore.load();
            pspcosttieditor.close();
        }
        else {
            factCalculationField.validate();
            planCalculationField.validate();
        }        
    },

    onOkpspcostprodeditorClick: function (button) {
        var pspcostprodeditor = button.up('pspcostprodeditor');
        var factCostProdField = pspcostprodeditor.down('[name=FactCostProd]');
        var planCostProdField = pspcostprodeditor.down('[name=PlanCostProd]');

        if (factCostProdField.isValid() && planCostProdField.isValid()) {
            var supportEditorWindow = Ext.ComponentQuery.query('custompromosupporteditor')[0];
            var promoLinkedViewer = supportEditorWindow.down('promolinkedviewer');
            var promoLinkedGrid = promoLinkedViewer.down('grid');
            var promoLinkedStore = promoLinkedGrid.getStore();
            var promoLinkedProxy = promoLinkedStore.getProxy();

            // ищем нужную запись
            for (var i = 0; i < promoLinkedProxy.data.length; i++) {
                if (promoLinkedProxy.data[i].id == pspcostprodeditor.pspId) {
                    promoLinkedProxy.data[i].set('PlanCostProd', planCostProdField.getValue());
                    promoLinkedProxy.data[i].set('FactCostProd', factCostProdField.getValue());
                }
            }

            promoLinkedStore.load();
            pspcostprodeditor.close();
        }
        else {
            factCostProdField.validate();
            planCostProdField.validate();
        }
    },
});
