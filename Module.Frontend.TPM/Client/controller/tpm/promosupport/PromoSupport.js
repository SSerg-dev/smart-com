Ext.define('App.controller.tpm.promosupport.PromoSupport', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'promosupport[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.onDetailPromoSupportClick
                },
                'promosupport directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'promosupport #datatable': {
                    activate: this.onActivateCard
                },
                'promosupport #detailform': {
                    activate: this.onActivateCard
                },
                'promosupport #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'promosupport #detailform #next': {
                    click: this.onNextButtonClick
                },
                'promosupport #detail': {
                    click: this.onDetailButtonClick
                },
                'promosupport #table': {
                    click: this.onTableButtonClick
                },
                'promosupport #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'promosupport #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'promosupport #createbutton': {
                    click: this.onCreateButtonClick
                },
                'promosupport #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'promosupport #updategroupbutton': {
                    click: this.onUpdateGroupButtonClick
                },
                'promosupport #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'promosupport #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'promosupport #refresh': {
                    click: this.onRefreshButtonClick
                },
                'promosupport #close': {
                    click: this.onCloseButtonClick
                },
                // import/export
                'promosupport #customexportxlsxbutton': {
                    click: this.onExportBtnClick
                },
                'promosupport #exportbutton': {
                    click: this.onExportButtonClick
                },
                'promosupport #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'promosupport #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'promosupport #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                },
                // PromoSupportType
                'promosupporttype': {
                    afterrender: this.onPromoSupportTypeAfterRender
                },
                'promosupporttype #ok': {
                    click: this.onPromoSupportTypeOkButtonClick
                },
                'promosupporttype button[itemId!=ok]': {
                    click: this.onSelectionButtonClick
                },

                // Promo Support Panel
                'promosupportpanel': {
                    render: this.onPromoSupportPanelRender
                },

                //PromoSupportForm
                'promosupportbottomtoolbar #savePromoSupportForm': {
                    click: this.onSavePromoSupportFormClick
                },
                'promosupportbottomtoolbar #cancelPromoSupportForm': {
                    click: this.onCancelPromoSupportFormClick
                },

                //Promo Support Left Toolbar
                'promosupportlefttoolbar #createPromoSupport': {
                    click: this.onCreatePromoSupportButtonClick
                },
                'promosupportlefttoolbar #createPromoSupportOnTheBasis': {
                    click: this.onCreatePromoSupportOnTheBasisButtonClick
                },
                'promosupportlefttoolbar #deletePromoSupport': {
                    click: this.onDeletePromoSupporButtonClick
                },
                'promosupportlefttoolbar #mainPromoSupportLeftToolbarContainer': {
                    add: this.onAddPromoSupportToolBar,
                    remove: this.onRemovePromoSupportToolBar
                },

                //закрытие окна
                'custompromosupporteditor': {
                    beforeclose: this.onBeforeClosePromoSupportEditor
                },
                'custompromosupporteditor #closePromoSupportEditorButton': {
                    click: this.onClosePromoSupportEditorButtonClick
                },
                'custompromosupporteditor #attachFile': {
                    click: this.onAttachFileButtonClick
                },
                'custompromosupporteditor #deleteAttachFile': {
                    click: this.onDeleteAttachFileButtonClick
                },
                '#customPromoSupportEditorUploadFileWindow #userOk': {
                    click: this.onUploadFileOkButtonClick
                },
                'custompromosupporteditor #editPromoSupportEditorButton': {
                    click: this.onEditPromoSupportEditorButton
                },

                //cost production
                'costproduction[isSearch!=true] directorygrid': {
                    itemdblclick: this.onDetailPromoSupportClick
                },
                'costproduction #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'costproduction #updategroupbutton': {
                    click: this.onUpdateGroupButtonClick
                },
            }
        });
    },

    onPromoSupportTypeAfterRender: function (window) {
        var closeButton = window.down('#close');
        var okButton = window.down('#ok');

        closeButton.setText(l10n.ns('tpm', 'PromoSupportType').value('ModalWindowCloseButton'));
        okButton.setText(l10n.ns('tpm', 'PromoSupportType').value('ModalWindowOkButton'));
    },

    onPromoSupportTypeOkButtonClick: function (button) {
        var window = button.up('window');
        var clientTreeField = window.down('treesearchfield[name=ClientTreeId]'),
            me = this;

        clientTreeField.validate();

        if (window.selectedButton && clientTreeField && clientTreeField.isValid()) {
            var promoSupportData = {
                clientField: clientTreeField,
                selectedButtonText: window.selectedButton.budgetRecord
            };

            //в зависимости от того, какая кнопка инициировала создание PromoSupport, нажатие на кнопку ОК в окне PromoSupportType 
            //приводит или к открытию формы создания PromoSupport или к появлению еще одной панельки в левом контейнере
            window.setLoading(true);
            if (window.createPromoSupportButton && window.createPromoSupportButton.itemId == 'createPromoSupport') {
                var editor = window.createPromoSupportButton.up('custompromosupporteditor');
                // Костыль для появления loading...
                setTimeout(this.addPromoSupportPanel, 0, promoSupportData, editor);
            } else {
                window.setLoading(true);
                $.ajax({
                    dataType: 'json',
                    url: '/odata/PromoSupports/GetUserTimestamp',
                    type: 'POST',
                    success: function (data) {
                        var customPromoSupportEditor = Ext.widget('custompromosupporteditor');
                        var choosenClient = {
                            fullPath: clientTreeField.rawValue,
                            id: clientTreeField.value
                        };

                        customPromoSupportEditor.choosenClient = choosenClient;
                        customPromoSupportEditor.userTimestamp = data.userTimestamp;

                        customPromoSupportEditor.show(null, function () {
                            window.setLoading(false);
                            window.close();
                        });

                        customPromoSupportEditor.down('#InvoiceNumber').show();

                        me.addPromoSupportPanel(promoSupportData, customPromoSupportEditor);
                        customPromoSupportEditor.down('#costProductionFieldset').setVisible(false);
                        window.setLoading(false);
                    },
                    error: function (data) {
                        window.setLoading(false);
                        App.Notify.pushError(data.statusText);
                    }
                });
            }
        } else {
            App.Notify.pushError('You should choose client and budget item before creating promo support.');
            window.setLoading(false);
        }
    },

    onCreateButtonClick: function (button) {
        var supportType = Ext.widget('promosupporttype');
        var mask = new Ext.LoadMask(supportType, { msg: "Please wait..." });
        var budgetName = 'Marketing TI';

        supportType.show();
        mask.show();

        supportType.createPromoSupportButton = button;

        var query = breeze.EntityQuery
            .from('BudgetItems')
            .using(Ext.ux.data.BreezeEntityManager.getEntityManager())
            .where("Budget.Name", "equals", budgetName)
            //.select("Name, Budget.Name")
            .expand('Budget')
            .execute()
            .then(function (data) {
                if (data.httpResponse.data.results.length > 0) {
                    data.httpResponse.data.results.forEach(function (item) {
                        // Контейнер с кнопкой (обводится бордером при клике)
                        var promoSupportTypeItem = Ext.widget({
                            extend: 'Ext.container.Container',
                            width: 'auto',
                            xtype: 'container',
                            layout: {
                                type: 'hbox',
                                align: 'stretch'
                            },
                            items: [{
                                xtype: 'button',
                                enableToggle: true,
                                cls: 'promo-support-type-select-list-button',
                            }]
                        });

                        promoSupportTypeItem.addCls('promo-support-type-select-list-container-button');

                        promoSupportTypeItem.down('button').style = { borderLeft: '6px solid ' + item.ButtonColor };
                        promoSupportTypeItem.down('button').setText(item.Name);
                        promoSupportTypeItem.down('button').budgetRecord = item;
                        supportType.down('fieldset').add(promoSupportTypeItem);
                    });
                } else {
                    Ext.ComponentQuery.query('promosupporttype')[0].close();
                    App.Notify.pushError('Не найдено записей для бюджета ' + budgetName);
                }

                mask.hide();
            })
            .fail(function () {
                App.Notify.pushError('Ошибка при выполнении операции');
                mask.hide();
            })

        // При создании из режима множественного редактирования нужно подставлять существующего клиента.
        var customPromoSupportEditor = Ext.ComponentQuery.query('custompromosupporteditor')[0];
        if (customPromoSupportEditor) {
            //var model = customPromoSupportEditor.promoSupportModel;
            var clientTreeIdTreeSearchField = supportType.down('[name=ClientTreeId]');

            clientTreeIdTreeSearchField.setDisabled(true);
            clientTreeIdTreeSearchField.setRawValue(customPromoSupportEditor.choosenClient.fullPath);
            clientTreeIdTreeSearchField.value = customPromoSupportEditor.choosenClient.id;
        }
    },

    //Promo Support Left Toolbar - CreatePromoSupport
    onCreatePromoSupportButtonClick: function (button) {
        this.onCreateButtonClick(button);
    },

    //Promo Support Left Toolbar - CreatePromoSupporOnTheBasis
    onCreatePromoSupportOnTheBasisButtonClick: function (button) {
        var me = this,
            editor = button.up('custompromosupporteditor'),
            mainContainer = editor.down('#mainPromoSupportLeftToolbarContainer'),
            promoSupportForm = editor.down('promosupportform'),
            startDateValue = promoSupportForm.down('datefield[name=StartDate]').getValue(),
            endDateValue = promoSupportForm.down('datefield[name=EndDate]').getValue();

        editor.setLoading(true);
        setTimeout(function () {
            editor.startDateValue = startDateValue;
            editor.endDateValue = endDateValue;

            if (editor && editor.promoSupportTypeData) {
                me.addPromoSupportPanel(editor.promoSupportTypeData, editor, true);
            } else if (editor) {
                var selectedItem,
                    data = new Object();

                mainContainer.items.items.forEach(function (item) {
                    if (item.hasCls('selected')) {
                        selectedItem = item;
                    }
                });

                data.budgetItemName = promoSupportForm.down('#promoSupportTypeField').getValue();
                data.clientFullPathName = selectedItem.clientFullPathName;
                data.clientId = selectedItem.clientId;
                data.budgetItemId = selectedItem.budgetItemId;
                data.borderColor = selectedItem.borderColor;

                me.addPromoSupportPanel(data, editor, true);
            }

            var promoLinkedStore = editor.down('promolinkedviewer').down('grid').getStore(),
                promoLinkedProxy = promoLinkedStore.getProxy();
            promoLinkedProxy.data = [];
            promoLinkedStore.load();

            editor.setLoading(false);
        }, 0);
    },

    onDeletePromoSupporButtonClick: function (button) {
        var mainContainer = button.up('promosupportlefttoolbar').down('#mainPromoSupportLeftToolbarContainer'),
            editor = button.up('custompromosupporteditor'),
            selectedItem;

        mainContainer.items.items.forEach(function (item) {
            if (item.hasCls('selected')) {
                selectedItem = item;
            }
        });

        if (selectedItem && selectedItem.model) {
            Ext.Msg.show({
                title: l10n.ns('core').value('deleteWindowTitle'),
                msg: l10n.ns('core').value('deleteConfirmMessage'),
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
                    editor.setLoading(l10n.ns('core').value('deletingText'));

                    selectedItem.model.destroy({
                        scope: this,
                        success: function () {
                            mainContainer.remove(selectedItem);
                            //выбор предыдущей записи в контейнере
                            var length = mainContainer.items.items.length,
                                prevPanel = mainContainer.items.items[length - 1];

                            if (length === 1) {
                                button.setDisabled(true);
                            }

                            this.clearPromoSupportForm(editor);

                            if (prevPanel) {
                                this.selectPromoSupportPanel(prevPanel);
                                this.updatePromoSupportForm(prevPanel);
                            }

                            editor.setLoading(false);
                        },
                        failure: function () {
                            editor.setLoading(false);
                        }
                    });
                }
            }
        }
    },

    onAddPromoSupportToolBar: function (toolbar, panel) {
        Ext.ComponentQuery.query('#promoSupportCounter')[0].setText(toolbar.items.items.length);
    },

    onRemovePromoSupportToolBar: function (toolbar) {
        Ext.ComponentQuery.query('#promoSupportCounter')[0].setText(toolbar.items.items.length);

        // панель с кнопками создания/удаления
        var btnPanel = toolbar.up('promosupportlefttoolbar');

        // если записей не осталось можем только создать
        if (toolbar.items.items.length == 0) {
            btnPanel.down('#deletePromoSupport').setDisabled(true);
            btnPanel.down('#createPromoSupportOnTheBasis').setDisabled(true);
            btnPanel.down('#createPromoSupport').setDisabled(false);

            var promoLinkedStore = toolbar.up('custompromosupporteditor').down('promolinkedviewer grid').getStore();
            var promoLinkedProxy = promoLinkedStore.getProxy();

            promoLinkedProxy.data = [];
            promoLinkedStore.load();
        }
    },

    onUpdateButtonClick: function (button) {
        var isCostProduction = button.up('costproduction') !== undefined;
        var customPromoSupportEditor = Ext.widget('custompromosupporteditor', { costProduction: isCostProduction });

        //скрываем левый тулбар
        customPromoSupportEditor.dockedItems.items[0].hide();

        var associatedContainer = button.up('#associatedpromosupportcontainer') || button.up('#associatedcostproductioncontainer');
        var promoLinkedGrid;
        //определяем, из какого пункта меню открыта форма (TI или Cost Production)
        //в зависимости от этого скрываем группу полей Cost Production или Parameters соответственно
        if (isCostProduction) {
            customPromoSupportEditor.down('#costProductionFieldset').setVisible(true);

            Ext.ComponentQuery.query('custompromosupporteditor field').forEach(function (field) {
                if (field.needReadOnlyFromCostProduction === true) {
                    field.setReadOnly(true);
                    field.addCls('readOnlyField');
                }
            });

            Ext.ComponentQuery.query('#promoSupportFormParameters')[0].hide();

            customPromoSupportEditor.down("#PONumber").show();
            // скрываем кнопки добавить и удалить в promolinkedviewer
            var promoLinkedViewer = customPromoSupportEditor.down('promolinkedviewer');
            promoLinkedViewer.addListener('afterrender', function () {
                var toolbarpromoLinked = promoLinkedViewer.down('custombigtoolbar');
                toolbarpromoLinked.down('#addbutton').hide();
                toolbarpromoLinked.down('#deletebutton').hide();
            });

            promoLinkedGrid = associatedContainer.down('promolinkedcostprod').down('grid');
        } else {
            customPromoSupportEditor.down("#InvoiceNumber").show();
            customPromoSupportEditor.down('#costProductionFieldset').setVisible(false);
            promoLinkedGrid = associatedContainer.down('promolinkedticosts').down('grid');
        }

        customPromoSupportEditor.width = '60%';
        //Иначе отобразиться только половинка окна
        customPromoSupportEditor.minWidth = 550;
        customPromoSupportEditor.down('#customPromoSupportEditorContainer').style = { borderLeft: 'none' };
        customPromoSupportEditor.singleUpdateMode = true;

        var promoSupportGrid = button.up('panel').down('grid'),
            promoLinkedStore = promoLinkedGrid.getStore(),
            count = promoLinkedStore.getCount(),
            selModel = promoSupportGrid.getSelectionModel(),
            promoLinkedRecords = count > 0 ? promoLinkedStore.getRange(0, count) : [];

        if (selModel.hasSelection()) {
            var selected = selModel.getSelection()[0];
            var promoLinkedViewerGrid = customPromoSupportEditor.down('promolinkedviewer grid');
            var promoLinkedViewerStore = promoLinkedViewerGrid.getStore();
            var promoLinkedViewerProxy = promoLinkedViewerStore.getProxy();
            var choosenClient = {
                fullPath: selected.data.ClientTreeFullPathName,
                id: selected.data.ClientTreeId
            };

            customPromoSupportEditor.PromoSupportPromoes = promoLinkedViewerProxy.getWriter().writeRecords(promoLinkedRecords, promoLinkedRecords.length).splice(0, promoLinkedRecords.length);
            customPromoSupportEditor.promoSupportModel = selected;
            customPromoSupportEditor.choosenClient = choosenClient;

            customPromoSupportEditor.show();
            this.fillSinglePromoSupportForm(customPromoSupportEditor);
        } else {
            App.Notify.pushInfo('No selection');
        }
    },

    fillSinglePromoSupportForm: function (editor) {
        var promoSupportForm = editor.down('promosupportform'),
            promoLinkedViewer = editor.down('promolinkedviewer'),
            model = editor.promoSupportModel;
        //заполнение поля имени клиента в хедере
        editor.down('promosupporttoptoolbar').down('label[name=client]').setText('Client: ' + model.data.ClientTreeFullPathName);

        //Promo Support Type (budgetItem)
        promoSupportTypeField = promoSupportForm.down('#promoSupportTypeField').setValue(model.data.BudgetSubItemBudgetItemName);

        // фильтр по BudgetItemId для SearchComboBox Equipment Type
        var budgetSubItemField = promoSupportForm.down('searchcombobox[name=BudgetSubItemId]'),
            budgetSubItemFieldStore = budgetSubItemField.getStore();
        budgetSubItemFieldStore.proxy.extraParams.ClientTreeId = model.data.ClientTreeId;
        budgetSubItemFieldStore.proxy.extraParams.BudgetItemId = model.data.BudgetSubItemBudgetItemId;
        //var parameters = {
        //    ClientTreeId: model.data.ClientTreeId
        //};
        //App.Util.makeRequestWithCallback('BudgetSubItemClientTrees', 'GetByClient', parameters, function (data) {
        //    var result = Ext.JSON.decode(data.httpResponse.data.value);
        //    budgetSubItemFieldStore.add(result.models);
        //    budgetSubItemField.setValue(model.data.BudgetSubItemId);
        //});
        budgetSubItemFieldStore.setFixedFilter('BudgetSubItemFilter', {
            property: 'BudgetItemId',
            operation: 'Equals',
            value: model.data.BudgetSubItemBudgetItemId
        })
        budgetSubItemField.setValue(model.data.BudgetSubItemId);

        //PONumber
        promoSupportForm.down('textfield[name=PONumber]').setValue(model.data.PONumber);

        //InvoiceNumber
        promoSupportForm.down('textfield[name=InvoiceNumber]').setValue(model.data.InvoiceNumber);

        //Off allocation
        var promoSupportTypeText = model.data.BudgetSubItemBudgetItemName;
        if (promoSupportTypeText === "Catalog") {
            var offAllocationCheckbox = promoSupportForm.down('checkboxfield[name=OffAllocationCheckbox]');
            offAllocationCheckbox.setVisible(true);
            offAllocationCheckbox.setValue(model.data.OffAllocation);
        } else {
            promoSupportForm.down('checkboxfield[name=OffAllocationCheckbox]').setVisible(false);
        }

        //Parameters
        promoSupportForm.down('numberfield[name=PlanQuantity]').setValue(model.data.PlanQuantity);
        promoSupportForm.down('numberfield[name=ActualQuantity]').setValue(model.data.ActualQuantity);
        promoSupportForm.down('numberfield[name=PlanCostTE]').setValue(model.data.PlanCostTE);
        promoSupportForm.down('numberfield[name=ActualCostTE]').setValue(model.data.ActualCostTE);

        //Period
        promoSupportForm.down('datefield[name=StartDate]').setValue(model.data.StartDate);
        promoSupportForm.down('datefield[name=EndDate]').setValue(model.data.EndDate);

        //Cost 
        promoSupportForm.down('numberfield[name=PlanProdCostPer1Item]').setValue(model.data.PlanProdCostPer1Item);
        promoSupportForm.down('numberfield[name=ActualProdCostPer1Item]').setValue(model.data.ActualProdCostPer1Item);
        promoSupportForm.down('numberfield[name=PlanProductionCost]').setValue(model.data.PlanProdCost);
        promoSupportForm.down('numberfield[name=ActualProductionCost]').setValue(model.data.ActualProdCost);

        //Attach
        var pattern = '/odata/PromoSupports/DownloadFile?fileName={0}';
        var downloadFileUrl = document.location.href + Ext.String.format(pattern, model.data.AttachFileName || '');
        promoSupportForm.down('#attachFileName').attachFileName = model.data.AttachFileName;
        promoSupportForm.down('#attachFileName').setValue(model.data.AttachFileName ? '<a href=' + downloadFileUrl + '>' + model.data.AttachFileName + '</a>' : "");

        var promoLinkedStore = promoLinkedViewer.down('grid').getStore();
        var promoLinkedProxy = promoLinkedStore.getProxy();
        var promoSupportPromoes = promoLinkedProxy.getReader().readRecords(editor.PromoSupportPromoes).records;

        promoLinkedProxy.data = promoSupportPromoes;
        promoLinkedStore.load();
        promoLinkedStore.load();
    },

    onUpdateGroupButtonClick: function (button) {
        var grid = button.up('panel').down('grid'),
            store = grid.getStore();
        var selModel = grid.getSelectionModel();
        var me = this;
        var associatedcontainer = grid.up('#associatedpromosupportcontainer') ||
            grid.up('#associatedcostproductioncontainer');

        associatedcontainer.setLoading(true);

        if (selModel.hasSelection()) {
            var selected = selModel.getSelection()[0];

            var parameters = {
                promoSupportId: breeze.DataType.Guid.fmtOData(selected.data.Id)
            };

            App.Util.makeRequestWithCallback('PromoSupports', 'GetPromoSupportGroup', parameters, function (data) {
                var result = Ext.JSON.decode(data.httpResponse.data.value);
                if (result.success) {
                    var isCostProduction = button.up('costproduction') !== undefined;
                    var customPromoSupportEditor = Ext.widget('custompromosupporteditor', { costProduction: isCostProduction });

                    if (isCostProduction) {
                        customPromoSupportEditor.down('#costProductionFieldset').setVisible(true);

                        Ext.ComponentQuery.query('custompromosupporteditor field').forEach(function (field) {
                            if (field.needReadOnlyFromCostProduction === true) {
                                field.setReadOnly(true);
                                field.addCls('readOnlyField');
                            }
                        });

                        Ext.ComponentQuery.query('#promoSupportFormParameters')[0].hide();

                        // скрываем кнопки добавить и удалить в promolinkedviewer
                        var promoLinkedViewer = customPromoSupportEditor.down('promolinkedviewer');
                        promoLinkedViewer.addListener('afterrender', function () {
                            var toolbarpromoLinked = promoLinkedViewer.down('custombigtoolbar');
                            toolbarpromoLinked.down('#addbutton').hide();
                            toolbarpromoLinked.down('#deletebutton').hide();
                        });

                        // скрываем кнопки управления группой                     
                        customPromoSupportEditor.down('#buttonPromoSupportLeftToolbarContainer').hide();

                        customPromoSupportEditor.down("#PONumber").show();

                    } else {
                        customPromoSupportEditor.down("#InvoiceNumber").show();
                        customPromoSupportEditor.down('#costProductionFieldset').setVisible(false);
                    }

                    var choosenClient = {
                        fullPath: selected.data.ClientTreeFullPathName,
                        id: selected.data.ClientTreeId
                    };

                    customPromoSupportEditor.choosenClient = choosenClient;
                    customPromoSupportEditor.promoSupportModel = selected;
                    customPromoSupportEditor.userTimestamp = selected.data.UserTimestamp;

                    if (result.models.length !== 0) {
                        var promoSupportModels = [];
                        result.models.forEach(function (model) {
                            var record = store.getById(model.PromoSupportId);
                            if (record) {
                                //record.PromoLinkedIds = model.PromoLinkedIds;

                                record.PromoSupportPromoes = model.PromoSupportPromoes;
                                promoSupportModels.push(record);
                            }
                        });
                        me.addPromoSupportPanelGroup(promoSupportModels, customPromoSupportEditor);
                        associatedcontainer.setLoading(false);
                    } else {
                        App.Notify.pushError('Group is empty');
                        associatedcontainer.setLoading(false);
                    }
                }
                else {
                    App.Notify.pushError(result.message);
                    associatedcontainer.setLoading(false);
                }
            });
        } else {
            App.Notify.pushInfo('No selection');
        }
    },

    addPromoSupportPanelGroup: function (modelArray, editor) {
        var mainContainer = editor.down('#mainPromoSupportLeftToolbarContainer');

        modelArray.forEach(function (model) {
            var promoSupportPanel = Ext.widget('promosupportpanel');
            promoSupportPanel.model = model;
            promoSupportPanel.promoSupportId = model.data.Id;
            promoSupportPanel.clientFullPathName = model.data.ClientTreeFullPathName;
            promoSupportPanel.clientId = model.data.ClientTreeId;
            promoSupportPanel.budgetItemId = model.data.BudgetSubItemBudgetItemId;
            promoSupportPanel.budgetSubItemId = model.data.BudgetSubItemId;
            promoSupportPanel.startDate = model.data.StartDate;
            promoSupportPanel.endDate = model.data.EndDate;
            promoSupportPanel.PromoSupportPromoes = model.PromoSupportPromoes;
            promoSupportPanel.planProdCostPer1Item = model.data.PlanProdCostPer1Item;
            promoSupportPanel.actualProdCostPer1Item = model.data.ActualProdCostPer1Item;
            promoSupportPanel.planProdCost = model.data.PlanProdCost;
            promoSupportPanel.actualProdCost = model.data.ActualProdCost;
            promoSupportPanel.attachFileName = model.data.AttachFileName;
            promoSupportPanel.borderColor = model.data.BorderColor;
            promoSupportPanel.planQuantity = model.data.PlanQuantity;
            promoSupportPanel.actualQuantity = model.data.ActualQuantity;
            promoSupportPanel.poNumber = model.data.PONumber;
            promoSupportPanel.invoiceNumber = model.data.InvoiceNumber;

            promoSupportPanel.down('#promoSupportTypeText').setText(model.data.BudgetSubItemBudgetItemName);
            promoSupportPanel.down('#promoLinkedText').setText(model.PromoSupportPromoes.length || '0');
            promoSupportPanel.down('#equipmentTypeText').setText(model.data.BudgetSubItemName);

            var startDate = new Date(promoSupportPanel.startDate),
                endDate = new Date(promoSupportPanel.endDate);
            promoSupportPanel.down('#periodText').setText(' c ' + Ext.Date.format(startDate, "d.m.Y") + ' по ' + Ext.Date.format(endDate, "d.m.Y"));

            promoSupportPanel.down('#planQuantityText').setText(model.data.PlanQuantity !== null ? model.data.PlanQuantity.toString() : null);
            promoSupportPanel.down('#planCostTEText').setText(model.data.PlanCostTE !== null ? model.data.PlanCostTE.toString() : null);
            promoSupportPanel.down('#actualQuantityText').setText(model.data.ActualQuantity !== null ? model.data.ActualQuantity.toString() : null);
            promoSupportPanel.down('#actualCostTEText').setText(model.data.ActualCostTE !== null ? model.data.ActualCostTE.toString() : null);
            promoSupportPanel.down('#attachFileText').setText(model.data.AttachFileName);

            promoSupportPanel.style = 'border-left: 5px solid ' + promoSupportPanel.borderColor;

            promoSupportPanel.saved = true;

            // Если эту строку убрать, то поломается удаление.
            promoSupportPanel.deletedPromoLinkedIds = [];
            mainContainer.add(promoSupportPanel);
        });

        editor.show();

        var topPanel = mainContainer.items.items[0];
        this.selectPromoSupportPanel(topPanel);
        this.clearPromoSupportForm(editor);
        this.updatePromoSupportForm(topPanel);
    },

    onSelectionButtonClick: function (button) {
        var window = button.up('window');
        var fieldsetWithButtons = window.down('fieldset');

        fieldsetWithButtons.items.items.forEach(function (item) {
            item.down('button').up('container').removeCls('promo-support-type-select-list-container-button-clicked');
        });

        button.up('container').addCls('promo-support-type-select-list-container-button-clicked');
        window.selectedButton = button;
    },

    addPromoSupportPanel: function (data, editor, onTheBasis) {
        var promoSupport = App.app.getController('tpm.promosupport.PromoSupport');
        var promoSupportTypeWindow = Ext.ComponentQuery.query('promosupporttype')[0];

        var mainContainer = editor.down('#mainPromoSupportLeftToolbarContainer'),
            promoSupportPanel = Ext.widget('promosupportpanel'),
            promoSupportForm = editor.down('promosupportform'),
            promoSupportTypeField = promoSupportForm.down('#promoSupportTypeField'),
            promoSupportTypeText = promoSupportPanel.down('#promoSupportTypeText'),
            budgetItem = data.selectedButtonText ? data.selectedButtonText.Name : data.budgetItemName,
            borderColor = data.selectedButtonText ? data.selectedButtonText.ButtonColor : data.borderColor;

        //добавление виджета в контейнер слева
        mainContainer.items.items.forEach(function (item) {
            if (item.hasCls('selected')) {
                item.removeCls('selected');
            }
        });

        promoSupportPanel.style = 'border-left: 5px solid ' + borderColor;
        mainContainer.add(promoSupportPanel);
        promoSupport.selectPromoSupportPanel(promoSupportPanel);

        //очистка формы
        promoSupport.clearPromoSupportForm(editor);
        //очистка promoSupportPanel
        var elementsToClear = promoSupportPanel.query('[needClear=true]');
        elementsToClear.forEach(function (el) {
            el.setText(null);
        });
        var promoLinkedText = promoSupportPanel.down('#promoLinkedText');
        promoLinkedText.setText('0');

        // заполняем поле клиента в заголовке
        var clientFullPathName = data.clientField ? data.clientField.rawValue : data.clientFullPathName;
        editor.down('promosupporttoptoolbar').down('label[name=client]').setText('Client: ' + clientFullPathName);
        editor.promoSupportTypeData = data;
        editor.clientId = data.clientField ? data.clientField.getValue() : data.clientId;
        editor.clientFullPathName = clientFullPathName;
        editor.borderColor = borderColor;

        // фильтр по BudgetItemId для SearchComboBox Equipment Type
        var budgetItemId = data.selectedButtonText ? data.selectedButtonText.Id : data.budgetItemId,
            budgetSubItemField = promoSupportForm.down('searchcombobox[name=BudgetSubItemId]'),
            budgetSubItemFieldStore = budgetSubItemField.getStore();
        promoSupportPanel.budgetItemId = budgetItemId;
        budgetSubItemFieldStore.proxy.extraParams.ClientTreeId = editor.clientId;
        budgetSubItemFieldStore.proxy.extraParams.BudgetItemId = budgetItemId;

        //var parameters = {
        //    ClientTreeId: editor.clientId
        //};
        //App.Util.makeRequestWithCallback('BudgetSubItemClientTrees', 'GetByClient', parameters, function (data) {
        //    var result = Ext.JSON.decode(data.httpResponse.data.value);
        //    budgetSubItemFieldStore.add(result.models);
        //});

        budgetSubItemFieldStore.setFixedFilter('BudgetSubItemFilter', {
            property: 'BudgetItemId',
            operation: 'Equals',
            value: budgetItemId
        })
        budgetSubItemFieldStore.load();

        promoSupportTypeField.setValue(budgetItem);
        promoSupportTypeText.setText(budgetItem);

        promoSupportPanel.saved = false;
        editor.promoSupportModel = null;

        if (onTheBasis) {
            promoSupportForm.down('datefield[name=StartDate]').setValue(editor.startDateValue);
            promoSupportForm.down('datefield[name=EndDate]').setValue(editor.endDateValue);

            var periodText = promoSupportPanel.down('#periodText');
            periodText.setText(' c ' + Ext.Date.format(editor.startDateValue, "d.m.Y") + ' по ' + Ext.Date.format(editor.endDateValue, "d.m.Y"));
        }

        if (promoSupportTypeWindow) {
            promoSupportTypeWindow.setLoading(false);
            promoSupportTypeWindow.close();
        }
    },

    onPromoSupportPanelRender: function (panel) {
        panel.body.on({
            click: { fn: this.onPromoSupportPanelClick, scope: panel }
        })
    },

    onPromoSupportPanelClick: function (el) {
        var panel = this,
            mainContainer = panel.up('#mainPromoSupportLeftToolbarContainer'),
            selectedItemId,
            selectedItem;

        panel.deletedPromoLinkedIds = [];

        mainContainer.items.items.forEach(function (item) {
            if (item.hasCls('selected')) {
                selectedItemId = item.id;
                selectedItem = item;
            }
        });

        // определяем текущий контроллер т.к. здесь this - это panel
        var me = App.app.getController('tpm.promosupport.PromoSupport');

        if (panel.id !== selectedItemId) {
            if (selectedItem.saved && !me.changedPromoSupport(selectedItem)) {
                selectedItem.removeCls('selected');
                me.selectPromoSupportPanel(panel);
                me.clearPromoSupportForm(panel.up('custompromosupporteditor'));
                me.updatePromoSupportForm(panel);
            } else {
                Ext.Msg.show({
                    title: 'Saving',//l10n.ns('core').value('deleteWindowTitle'),
                    msg: 'Would you like to save current item?',//l10n.ns('core').value('deleteConfirmMessage'),
                    fn: onMsgBoxClose,
                    scope: this,
                    icon: Ext.Msg.QUESTION,
                    buttons: Ext.Msg.YESNO,
                    buttonText: {
                        yes: 'Yes',//l10n.ns('core', 'buttons').value('delete'),
                        no: 'No'//l10n.ns('core', 'buttons').value('cancel')
                    }
                });

                function onMsgBoxClose(buttonId) {
                    if (buttonId === 'yes') {
                        var saveButton = panel.up('custompromosupporteditor').down('#savePromoSupportForm');
                        me.SavePromoSupport(saveButton, function () {
                            selectedItem.removeCls('selected');
                            me.selectPromoSupportPanel(panel);
                            me.clearPromoSupportForm(panel.up('custompromosupporteditor'));
                            me.updatePromoSupportForm(panel);
                        });
                    } else if (buttonId === 'no') {
                        var editor = panel.up('custompromosupporteditor');

                        if (selectedItem.saved) {
                            selectedItem.removeCls('selected');
                            me.clearPromoSupportForm(editor);
                            me.selectPromoSupportPanel(panel);
                            me.updatePromoSupportForm(panel);
                        }
                        else {
                            me.cancelUnsavePromoSupport(editor, selectedItem);
                        }
                    }
                }
            }
        };
    },

    updatePromoSupportForm: function (panel) {
        var editor = panel.up('custompromosupporteditor'),
            promoLinkedViewer = editor.down('promolinkedviewer'),
            promoSupportForm = editor.down('promosupportform');

        editor.promoSupportModel = panel.model;

        // заполняем поле клиента в заголовке
        var clientFullPathName = panel.clientFullPathName;
        editor.down('promosupporttoptoolbar').down('label[name=client]').setText('Client: ' + clientFullPathName);
        editor.promoSupportTypeData = panel.promoSupportTypeData;
        editor.clientId = panel.clientId;
        editor.clientFullPathName = panel.clientFullPathName;

        // фильтр по BudgetItemId для SearchComboBox Equipment Type
        var budgetItemId = panel.budgetItemId,
            budgetSubItemField = promoSupportForm.down('searchcombobox[name=BudgetSubItemId]'),
            budgetSubItemFieldStore = budgetSubItemField.getStore();
        budgetSubItemFieldStore.proxy.extraParams.ClientTreeId = editor.clientId;
        budgetSubItemFieldStore.proxy.extraParams.BudgetItemId = budgetItemId;
        //var parameters = {
        //    ClientTreeId: editor.clientId
        //};
        //App.Util.makeRequestWithCallback('BudgetSubItemClientTrees', 'GetByClient', parameters, function (data) {
        //    var result = Ext.JSON.decode(data.httpResponse.data.value);
        //    budgetSubItemFieldStore.add(result.models);
        //    budgetSubItemField.setValue(model.data.BudgetSubItemId);
        //});


        budgetSubItemFieldStore.setFixedFilter('BudgetSubItemFilter', {
            property: 'BudgetItemId',
            operation: 'Equals',
            value: budgetItemId
        })
        budgetSubItemField.setValue(panel.budgetSubItemId);

        //Promo Support Type (budgetItem)
        var promoSupportTypeText = panel.down('#promoSupportTypeText').text;
        promoSupportTypeField = promoSupportForm.down('#promoSupportTypeField').setValue(promoSupportTypeText);

        //PONumber
        promoSupportForm.down('textfield[name=PONumber]').setValue(panel.poNumber);

        //InvoiceNumber
        promoSupportForm.down('textfield[name=InvoiceNumber]').setValue(panel.invoiceNumber);


        //Parameters
        var planQuantityText = panel.down('#planQuantityText').text,
            actualQuantityText = panel.down('#actualQuantityText').text,
            planCostTEText = panel.down('#planCostTEText').text,
            actualCostTEText = panel.down('#actualCostTEText').text,
            attachFileText = panel.down('#attachFileText').text;

        promoSupportForm.down('numberfield[name=PlanQuantity]').setValue(planQuantityText);
        promoSupportForm.down('numberfield[name=ActualQuantity]').setValue(actualQuantityText);
        promoSupportForm.down('numberfield[name=PlanCostTE]').setValue(planCostTEText);
        promoSupportForm.down('numberfield[name=ActualCostTE]').setValue(actualCostTEText);
        promoSupportForm.down('#attachFileName').setValue(attachFileText);

        //Period
        promoSupportForm.down('datefield[name=StartDate]').setValue(panel.startDate);
        promoSupportForm.down('datefield[name=EndDate]').setValue(panel.endDate);

        //Cost 
        promoSupportForm.down('numberfield[name=PlanProdCostPer1Item]').setValue(panel.planProdCostPer1Item);
        promoSupportForm.down('numberfield[name=ActualProdCostPer1Item]').setValue(panel.actualProdCostPer1Item);
        promoSupportForm.down('numberfield[name=PlanProductionCost]').setValue(panel.planProdCost);
        promoSupportForm.down('numberfield[name=ActualProductionCost]').setValue(panel.actualProdCost);

        //Attach
        var pattern = '/odata/PromoSupports/DownloadFile?fileName={0}';
        var downloadFileUrl = document.location.href + Ext.String.format(pattern, panel.attachFileName || '');
        promoSupportForm.down('#attachFileName').attachFileName = panel.attachFileName;
        promoSupportForm.down('#attachFileName').setValue('<a href=' + downloadFileUrl + '>' + panel.attachFileName || '' + '</a>');
    },

    clearPromoSupportForm: function (editor) {
        var promoLinkedViewer = editor.down('promolinkedviewer'),
            promoSupportForm = editor.down('promosupportform');

        //очистка полей на форме PromoSupportForm
        var elementsToClear = promoSupportForm.query('[needClear=true]');
        elementsToClear.forEach(function (el) {
            el.setValue(null);
        });

        var attachFileField = editor.down('#attachFileName');
        attachFileField.setValue('');
        attachFileField.attachFileName = '';
    },

    onSavePromoSupportFormClick: function (button) {
        this.SavePromoSupport(button, null);
    },

    SavePromoSupport: function (button, callback) {
        var me = this,
            editor = button.up('custompromosupporteditor');

        if (/*me.checkSummOfValues(editor) &&*/ me.validateFields(editor)) {
            editor.setLoading(l10n.ns('core').value('savingText'));
            setTimeout(function () {

                if (me.checkPromoDates(editor)) {
                    me.generateAndSendModel(editor, callback, me);
                }
                else {
                    Ext.Msg.show({
                        title: l10n.ns('core').value('confirmTitle'),
                        msg: l10n.ns('tpm', 'PromoSupportPromo').value('ConfirmNewDates'),
                        fn: onMsgBoxClose,
                        scope: this,
                        icon: Ext.Msg.QUESTION,
                        buttons: Ext.Msg.YESNO,
                        buttonText: {
                            yes: l10n.ns('core', 'buttons').value('save'),
                            no: l10n.ns('core', 'buttons').value('cancel')
                        }
                    });

                    function onMsgBoxClose(buttonId) {
                        if (buttonId === 'yes') {
                            me.generateAndSendModel(editor, callback, me);
                        }
                        else {
                            editor.setLoading(false);
                        }
                    }
                }

            }, 0);
        }
    },

    generateAndSendModel: function (editor, callback, scope) {
        var me = scope,
            promoLinkedViewer = editor.down('promolinkedviewer'),
            promoSupportForm = editor.down('promosupportform'),
            mainContainer = editor.down('#mainPromoSupportLeftToolbarContainer'),
            promoLinkedGrid = promoLinkedViewer.down('grid'),
            promoLinkedStore = promoLinkedGrid.getStore(),
            countStore = promoLinkedStore.getProxy().data.length;

        //PONumber
        var poNumber = promoSupportForm.down('textfield[name=PONumber]').getValue();

        if (!poNumber) {
            poNumber = '';
            promoSupportForm.down('textfield[name=PONumber]').setValue('');
        }

        //InvoiceNumber
        var invoiceNumber = promoSupportForm.down('textfield[name=InvoiceNumber]').getValue();

        if (!invoiceNumber) {
            invoiceNumber = '';
            promoSupportForm.down('textfield[name=InvoiceNumber]').setValue('');
        }

        //поля на форме PromoSupportForm
        //Parameters
        var planQuantityValue = promoSupportForm.down('numberfield[name=PlanQuantity]').getValue(),
            actualQuantityValue = promoSupportForm.down('numberfield[name=ActualQuantity]').getValue(),
            planCostTEValue = promoSupportForm.down('numberfield[name=PlanCostTE]').getValue(),
            actualCostTEValue = promoSupportForm.down('numberfield[name=ActualCostTE]').getValue();

        // какие-то проблемы с 0 и Null, в БД Null не бывает поэтому: (можно и дефолт поставить, но не будем)
        if (!planQuantityValue) {
            planQuantityValue = 0;
            promoSupportForm.down('numberfield[name=PlanQuantity]').setValue(0);
        }

        if (!actualQuantityValue) {
            actualQuantityValue = 0;
            promoSupportForm.down('numberfield[name=ActualQuantity]').setValue(0);
        }

        if (!planCostTEValue) {
            planCostTEValue = 0;
            promoSupportForm.down('numberfield[name=PlanCostTE]').setValue(0);
        }

        if (!actualCostTEValue) {
            actualCostTEValue = 0;
            promoSupportForm.down('numberfield[name=ActualCostTE]').setValue(0);
        }

        //Period
        var startDateValueField = promoSupportForm.down('datefield[name=StartDate]');
        var endDateValueField = promoSupportForm.down('datefield[name=EndDate]');

        var startDateValue = startDateValueField.getValue(),
            endDateValue = endDateValueField.getValue();

        //startDateValueField.validate();
        //endDateValueField.validate();

        //Cost
        // либо костыль, либо все переделать
        var planProdCostPer1ItemValue = 0,
            actualProdCostPer1ItemValue = 0;

        if (promoSupportForm.down('numberfield[name=PlanProdCostPer1Item]')) {
            planProdCostPer1ItemValue = promoSupportForm.down('numberfield[name=PlanProdCostPer1Item]').getValue();
            actualProdCostPer1ItemValue = promoSupportForm.down('numberfield[name=ActualProdCostPer1Item]').getValue();
        }

        var planProdCostValue = 0,
            actualProdCostValue = 0;

        if (promoSupportForm.down('numberfield[name=PlanProductionCost]')) {
            planProdCostValue = promoSupportForm.down('numberfield[name=PlanProductionCost]').getValue();
            actualProdCostValue = promoSupportForm.down('numberfield[name=ActualProductionCost]').getValue();
        }

        /*
        if (!planProdCostValue) {
            planProdCostValue = 0;
            promoSupportForm.down('numberfield[name=PlanProductionCost]').setValue(0);
        }

        if (!actualProdCostValue) {
            actualProdCostValue = 0;
            promoSupportForm.down('numberfield[name=ActualProductionCost]').setValue(0);
        }
        */

        //Attach
        var attachFileField = promoSupportForm.down('#attachFileName');
        var attachFileName = attachFileField.attachFileName !== undefined && attachFileField.attachFileName !== null
            ? attachFileField.attachFileName : "";

        //поиск текущего PromoSupport в панели слева
        var currentPromoSupport;
        mainContainer.items.items.forEach(function (item) {
            if (item.hasCls('selected')) {
                currentPromoSupport = item;
            }
        });

        //Заполнение модели и сохранение записи
        var model = editor.promoSupportModel ? editor.promoSupportModel : Ext.create('App.model.tpm.promosupport.PromoSupport'),
            budgetSubItemField = promoSupportForm.down('searchcombobox[name=BudgetSubItemId]'),
            budgetSubItemId = budgetSubItemField.getValue(),
            budgetSubItemName = budgetSubItemField.rawValue;

        model.editing = true;
        //budgetSubItemField.validate();

        if (currentPromoSupport && budgetSubItemId) {
            var promoLinkedText = currentPromoSupport.down('#promoLinkedText');
            promoLinkedText.setText(countStore ? countStore : '0');

            //Equipment Type
            var equipmentTypeText = currentPromoSupport.down('#equipmentTypeText');
            equipmentTypeText.setText(budgetSubItemName);

            //Parameters
            currentPromoSupport.down('#planQuantityText').setText(planQuantityValue !== null ? planQuantityValue.toString() : null);
            currentPromoSupport.down('#actualQuantityText').setText(actualQuantityValue !== null ? actualQuantityValue.toString() : null);
            currentPromoSupport.down('#planCostTEText').setText(planCostTEValue !== null ? planCostTEValue.toString() : null);
            currentPromoSupport.down('#actualCostTEText').setText(actualCostTEValue !== null ? actualCostTEValue.toString() : null);
            currentPromoSupport.down('#attachFileText').setText(attachFileName);

            //Period
            currentPromoSupport.down('#periodText').setText(' c ' + Ext.Date.format(startDateValue, "d.m.Y") + ' по ' + Ext.Date.format(endDateValue, "d.m.Y"));

            model.data.Id = currentPromoSupport.promoSupportId ? currentPromoSupport.promoSupportId : model.data.Id;
            model.set('PONumber', poNumber);
            model.set('InvoiceNumber', invoiceNumber);
            model.set('ClientTreeId', editor.clientId);
            model.set('BudgetSubItemId', budgetSubItemId);
            model.set('PlanQuantity', planQuantityValue);
            model.set('ActualQuantity', actualQuantityValue);
            model.set('PlanCostTE', planCostTEValue);
            model.set('ActualCostTE', actualCostTEValue);
            model.set('StartDate', startDateValue);
            model.set('EndDate', endDateValue);
            model.set('PlanProdCostPer1Item', planProdCostPer1ItemValue);
            model.set('ActualProdCostPer1Item', actualProdCostPer1ItemValue);
            model.set('PlanProdCost', planProdCostValue);
            model.set('ActualProdCost', actualProdCostValue);
            model.set('UserTimestamp', editor.userTimestamp ? editor.userTimestamp : null);
            model.set('AttachFileName', attachFileName);

            if (editor.borderColor) {
                model.set('BorderColor', editor.borderColor);
            }

            //подготовка данных для заполнения таблицы PromoSuportPromo(привязка выбранных Promo с создаваемому PromoSupport)
            var count = promoLinkedStore.getCount(),
                promoLinkedRecords = count > 0 ? promoLinkedStore.getRange(0, count) : [],
                promoIdString = '',
                checkedRowsIds = [];

            //привязка параметров сохраняемого Promo Support к панельке в левой части экрана(чтобы при клике на нее заполнить форму)
            currentPromoSupport.promoSupportTypeData = editor.promoSupportTypeData;
            currentPromoSupport.clientId = editor.clientId;
            currentPromoSupport.clientFullPathName = editor.clientFullPathName;
            currentPromoSupport.budgetSubItemId = budgetSubItemId;
            currentPromoSupport.startDate = startDateValue;
            currentPromoSupport.endDate = endDateValue;
            currentPromoSupport.planProdCostPer1Item = planProdCostPer1ItemValue;
            currentPromoSupport.actualProdCostPer1Item = actualProdCostPer1ItemValue;
            currentPromoSupport.planProdCost = planProdCostValue;
            currentPromoSupport.actualProdCost = actualProdCostValue;
            currentPromoSupport.attachFileName = attachFileName;
            currentPromoSupport.borderColor = editor.borderColor;

            editor.setLoading(l10n.ns('core').value('savingText'));

            model.save({
                scope: me,
                success: function () {
                    model.set('ClientTreeFullPathName', editor.clientFullPathName);

                    currentPromoSupport.modelId = model.data.Id;
                    currentPromoSupport.model = model;
                    currentPromoSupport.saved = true;
                    editor.promoSupportModel = model;

                    editor.down('#createPromoSupport').setDisabled(false);
                    editor.down('#createPromoSupportOnTheBasis').setDisabled(false);
                    editor.down('#deletePromoSupport').setDisabled(false);

                    var pspMin = []

                    if (promoLinkedRecords.length > 0 && promoLinkedStore.getCount() !== 0) {
                        promoLinkedRecords.forEach(function (record) {
                            pspMin.push({
                                Id: record.data.Id ? record.data.Id : '00000000-0000-0000-0000-000000000000',
                                PromoId: record.data.PromoId,
                                PromoSupportId: model.data.Id,
                                FactCalculation: record.data.FactCalculation,
                                FactCostProd: record.data.FactCostProd,
                                PlanCalculation: record.data.PlanCalculation,
                                PlanCostProd: record.data.PlanCostProd
                            });
                        });
                    }

                    $.ajax({
                        type: "POST",
                        cache: false,
                        url: "/odata/PromoSupportPromoes/ChangeListPSP?promoSupportId=" + model.data.Id,
                        data: JSON.stringify(pspMin),
                        dataType: "json",
                        contentType: false,
                        processData: false,
                        success: function (response) {
                            var result = Ext.JSON.decode(response.value);
                            if (result.success) {
                                var promoLinkedProxy = promoLinkedStore.getProxy();

                                // только если вкладка та же
                                if (currentPromoSupport.hasCls('selected')) {
                                    var promoSupportPromoes = promoLinkedProxy.getReader().readRecords(result.list).records;
                                    promoLinkedProxy.data = promoSupportPromoes;
                                    promoLinkedStore.load();
                                }

                                currentPromoSupport.PromoSupportPromoes = result.list;
                                editor.setLoading(false);
                            }
                            else {
                                App.Notify.pushError(result.message);
                                editor.setLoading(false);
                            }
                        }
                    });                    

                    if (callback) {
                        callback();
                    }
                },
                failure: function () {
                    editor.setLoading(false);
                }
            })
        } else if (editor.singleUpdateMode) {
            model.set('PONumber', poNumber);
            model.set('InvoiceNumber', invoiceNumber);
            model.set('BudgetSubItemId', budgetSubItemId);
            model.set('PlanQuantity', planQuantityValue);
            model.set('ActualQuantity', actualQuantityValue);
            model.set('PlanCostTE', planCostTEValue);
            model.set('ActualCostTE', actualCostTEValue);
            model.set('StartDate', startDateValue);
            model.set('EndDate', endDateValue);
            model.set('PlanProdCostPer1Item', planProdCostPer1ItemValue);
            model.set('ActualProdCostPer1Item', actualProdCostPer1ItemValue);
            model.set('PlanProdCost', planProdCostValue);
            model.set('ActualProdCost', actualProdCostValue);
            model.set('AttachFileName', attachFileName);

            //подготовка данных для заполнения таблицы PromoSuportPromo(привязка выбранных Promo с создаваемому PromoSupport)
            var count = promoLinkedStore.getCount(),
                promoLinkedRecords = count > 0 ? promoLinkedStore.getRange(0, count) : [],
                promoIdString = '',
                checkedRowsIds = [];

            model.save({
                scope: me,
                success: function () {

                    var pspMin = []

                    if (promoLinkedRecords.length > 0) {
                        promoLinkedRecords.forEach(function (record) {
                            pspMin.push({
                                Id: record.data.Id ? record.data.Id : '00000000-0000-0000-0000-000000000000',
                                PromoId: record.data.PromoId,
                                PromoSupportId: model.data.Id,
                                FactCalculation: record.data.FactCalculation,
                                FactCostProd: record.data.FactCostProd,
                                PlanCalculation: record.data.PlanCalculation,
                                PlanCostProd: record.data.PlanCostProd
                            });
                        });
                    }

                    $.ajax({
                        type: "POST",
                        cache: false,
                        url: "/odata/PromoSupportPromoes/ChangeListPSP?promoSupportId=" + model.data.Id,
                        data: JSON.stringify(pspMin),
                        dataType: "json",
                        contentType: false,
                        processData: false,
                        success: function (response) {
                            var result = Ext.JSON.decode(response.value);
                            if (result.success) {
                                var promoLinkedProxy = promoLinkedStore.getProxy();
                                var promoSupportPromoes = promoLinkedProxy.getReader().readRecords(result.list).records;

                                promoLinkedProxy.data = promoSupportPromoes;
                                promoLinkedStore.load();
                                editor.PromoSupportPromoes = result.list;
                                editor.setLoading(false);
                            }
                            else {
                                App.Notify.pushError(result.message);
                                editor.setLoading(false);
                            }
                        }
                    });

                    if (callback) {
                        callback();
                    }
                },
                failure: function () {
                    editor.setLoading(false);
                }
            })
        } else {
            editor.setLoading(false);
        }
    },

    onCancelPromoSupportFormClick: function (button) {
        var me = this;
        var editor = button.up('custompromosupporteditor');
        var mainContainer = editor.down('#mainPromoSupportLeftToolbarContainer');
        var selectedItem;

        editor.setLoading(true);
        setTimeout(function () {
            mainContainer.items.items.forEach(function (item) {
                // Очистка удаленных записей, хранящихся в панельках (множественное редактирование)
                item.deletedPromoLinkedIds = [];

                if (item.hasCls('selected')) {
                    selectedItem = item;
                }
            });

            // Очистка удаленных записей, хранящихся в promolinkedviewer (единичное редактирование)
            var promoLinkedViewer = Ext.ComponentQuery.query('promolinkedviewer')[0]
            promoLinkedViewer.deletedPromoLinkedIds = [];

            if (editor.singleUpdateMode) {
                me.fillSinglePromoSupportForm(editor);

                // дисейблим поля
                Ext.ComponentQuery.query('custompromosupporteditor field').forEach(function (field) {
                    field.setReadOnly(true);
                    field.addCls('readOnlyField');
                });

                // кнопки прикрепления файла
                editor.down('#attachFile').setDisabled(true);
                editor.down('#deleteAttachFile').setDisabled(true);

                editor.down('#editPromoSupportEditorButton').setVisible(true);
                editor.down('#savePromoSupportForm').setVisible(false);
                editor.down('#cancelPromoSupportForm').setVisible(false);

                var promoLinkedViewer = editor.down('promolinkedviewer'),
                    toolbarpromoLinked = promoLinkedViewer.down('custombigtoolbar');
                toolbarpromoLinked.down('#addbutton').setDisabled(true);
                //toolbarpromoLinked.down('#updatebutton').setDisabled(true);
                toolbarpromoLinked.down('#deletebutton').setDisabled(true);
            }
            else if (selectedItem) {
                if (selectedItem.saved) {
                    me.clearPromoSupportForm(editor);
                    me.selectPromoSupportPanel(selectedItem);
                    me.updatePromoSupportForm(selectedItem);
                } else {
                    me.cancelUnsavePromoSupport(editor, selectedItem);
                }
            }

            editor.setLoading(false);
        }, 0);
    },

    cancelUnsavePromoSupport: function (editor, selectedItem) {
        var mainContainer = editor.down('#mainPromoSupportLeftToolbarContainer');

        if (!selectedItem.saved) {
            mainContainer.remove(selectedItem);
            //выбор предыдущей записи в контейнере
            var length = mainContainer.items.items.length,
                prevPanel = mainContainer.items.items[length - 1];            
            
            this.clearPromoSupportForm(editor);

            if (prevPanel) {
                this.selectPromoSupportPanel(prevPanel);
                this.updatePromoSupportForm(prevPanel);
            }
        }
    },

    onClosePromoSupportEditorButtonClick: function (button) {
        var editor = button.up('custompromosupporteditor');       

        editor.close();
    },

    onAttachFileButtonClick: function (button) {
        var resource = 'PromoSupports';
        var action = 'UploadFile';

        var uploadFileWindow = Ext.widget('uploadfilewindow', {
            itemId: 'customPromoSupportEditorUploadFileWindow',
            resource: resource,
            action: action,
            buttons: [{
                text: l10n.ns('core', 'buttons').value('cancel'),
                itemId: 'cancel'
            }, {
                text: l10n.ns('core', 'buttons').value('upload'),
                ui: 'green-button-footer-toolbar',
                itemId: 'userOk'
            }]
        });

        uploadFileWindow.show();
    },

    onDeleteAttachFileButtonClick: function (button) {
        var attachFileName = button.up('custompromosupporteditor').down('#attachFileName'); 
        var editor = button.up('custompromosupporteditor');

        attachFileName.setValue('');
        attachFileName.attachFileName = '';
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
                        var pattern = '/odata/PromoSupports/DownloadFile?fileName={0}';
                        var downloadFileUrl = document.location.href + Ext.String.format(pattern, o.result.fileName);
                        var custompromosupporteditor = Ext.ComponentQuery.query('custompromosupporteditor')[0];

                        Ext.ComponentQuery.query('#attachFileName')[0].setValue('<a href=' + downloadFileUrl + '>' + o.result.fileName + '</a>');
                        Ext.ComponentQuery.query('#attachFileName')[0].attachFileName = o.result.fileName;

                        App.Notify.pushInfo(win.successMessage || 'Файл был загружен на сервер');
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

    onAttachFileNameClick: function () {
    },

    // переопределение нажатия кнопки экспорта, для определения раздела (TI Cost/Cost Production)
    onExportBtnClick: function (button) {
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

    // функция выбора редактируемой записи (касается визуальной части)
    selectPromoSupportPanel: function (panel) {
        var mainContainer = panel.up('#mainPromoSupportLeftToolbarContainer');        
        var btnPanel = mainContainer.up('promosupportlefttoolbar'); // Панель с кнопками создания/удаления

        panel.addCls('selected');
        btnPanel.down('#deletePromoSupport').setDisabled(!panel.saved);
        btnPanel.down('#createPromoSupport').setDisabled(!panel.saved);
        btnPanel.down('#createPromoSupportOnTheBasis').setDisabled(!panel.saved);

        var promoLinkedStore = panel.up('custompromosupporteditor').down('promolinkedviewer grid').getStore();
        var promoLinkedProxy = promoLinkedStore.getProxy();

        if (panel.PromoSupportPromoes) {            
            var promoSupportPromoes = promoLinkedProxy.getReader().readRecords(panel.PromoSupportPromoes).records;

            promoLinkedProxy.data = promoSupportPromoes;
            promoLinkedStore.load();
        }
        else {
            promoLinkedProxy.data = [];
            promoLinkedStore.load();
        }
    },

    // возвращает true, если появились изменения
    changedPromoSupport: function (promoSupportPanel) {
        var result = true;
        var editor = promoSupportPanel.up('custompromosupporteditor');
        var promoSupportForm = editor.down('promosupportform');
        var promoLinkedViewer = editor.down('promolinkedviewer');
        var promoLinkedStore = promoLinkedViewer.down('grid').getStore();            

        //поля на форме PromoSupportForm
        //Parameters
        var planQuantityValue = promoSupportForm.down('numberfield[name=PlanQuantity]').getValue();
        var actualQuantityValue = promoSupportForm.down('numberfield[name=ActualQuantity]').getValue();
        var planCostTEValue = promoSupportForm.down('numberfield[name=PlanCostTE]').getValue();
        var actualCostTEValue = promoSupportForm.down('numberfield[name=ActualCostTE]').getValue();

        //Period
        var startDateValue = promoSupportForm.down('datefield[name=StartDate]').getValue();
        var endDateValue = promoSupportForm.down('datefield[name=EndDate]').getValue();

        //Cost
        var planProdCostPer1Item = promoSupportForm.down('numberfield[name=PlanProdCostPer1Item]').getValue();
        var actualProdCostPer1Item = promoSupportForm.down('numberfield[name=ActualProdCostPer1Item]').getValue();
        var planProdCostValue = promoSupportForm.down('numberfield[name=PlanProductionCost]').getValue();
        var actualProdCostValue = promoSupportForm.down('numberfield[name=ActualProductionCost]').getValue();

        var attachFileName = promoSupportForm.down('#attachFileName').attachFileName;
        var budgetSubItemField = promoSupportForm.down('searchcombobox[name=BudgetSubItemId]');
        var budgetSubItemId = budgetSubItemField.getValue(),

        result = result && promoSupportPanel.model.data.ActualCostTE == actualCostTEValue;
        result = result && promoSupportPanel.model.data.PlanCostTE == planCostTEValue;
        result = result && promoSupportPanel.model.data.ActualQuantity == actualQuantityValue;
        result = result && promoSupportPanel.model.data.PlanQuantity == planQuantityValue;
        result = result && promoSupportPanel.model.data.AttachFileName == attachFileName;        
        result = result && promoSupportPanel.model.data.BudgetSubItemId == budgetSubItemId;
        result = result && promoSupportPanel.model.data.StartDate.toString() == startDateValue.toString();
        result = result && promoSupportPanel.model.data.EndDate.toString() == endDateValue.toString(); 
        result = result && promoSupportPanel.model.data.ActualProdCostPer1Item == actualProdCostPer1Item;
        result = result && promoSupportPanel.model.data.PlanProdCostPer1Item == planProdCostPer1Item;
        result = result && promoSupportPanel.model.data.ActualProdCost == actualProdCostValue;
        result = result && promoSupportPanel.model.data.PlanProdCost == planProdCostValue;

        // проверка изменений прикрепленных промо
        var newLinkedPromoCount = promoLinkedStore.getCount();
        var promoLinkedRecords = newLinkedPromoCount > 0 ? promoLinkedStore.getRange(0, newLinkedPromoCount) : [];            
        var checkedRowsIds = [];

        if (promoLinkedRecords.length > 0) {
            promoLinkedRecords.forEach(function (record) {
                checkedRowsIds.push(record.data.Id);
            });
        }

        var intersect = promoSupportPanel.PromoSupportPromoes.filter(function (n) {
            return checkedRowsIds.indexOf(n.Id) !== -1;
        });

        result = result && promoSupportPanel.PromoSupportPromoes.length == intersect.length
            && checkedRowsIds.length == intersect.length;

        // если все верно осталось проверить только изменения распределенных значений для промо
        if (result) {
            var promoLinkedProxy = promoLinkedStore.getProxy();
            promoSupportPanel.PromoSupportPromoes.forEach(function (currentPsp) {
                var pspInStore = promoLinkedProxy.data.find(function (n) { return n.data.Id == currentPsp.Id })

                if (pspInStore.get('PlanCostProd') != currentPsp.PlanCostProd ||
                    pspInStore.get('FactCostProd') != currentPsp.FactCostProd ||
                    pspInStore.get('PlanCalculation') != currentPsp.PlanCalculation ||
                    pspInStore.get('FactCalculation') != currentPsp.FactCalculation)
                {
                    result = false;
                }
            });
        }

        return !result;
    },

    onDetailPromoSupportClick: function (item) {
        var isCostProduction = item.up('costproduction') !== undefined;
        var customPromoSupportEditor = Ext.widget('custompromosupporteditor', { costProduction: isCostProduction });

        //скрываем левый тулбар
        customPromoSupportEditor.dockedItems.items[0].hide();

        // дисейблим поля
        Ext.ComponentQuery.query('custompromosupporteditor field').forEach(function (field) {
            field.setReadOnly(true);
            field.addCls('readOnlyField');
        });

        var promoLinkedGrid;
        var associatedContainer = item.up('#associatedpromosupportcontainer') || item.up('#associatedcostproductioncontainer');
        //определяем, из какого пункта меню открыта форма (TI или Cost Production)
        //в зависимости от этого скрываем группу полей Cost Production или Parameters соответственно
        if (isCostProduction) {
            customPromoSupportEditor.down('#costProductionFieldset').setVisible(true);
            Ext.ComponentQuery.query('#promoSupportFormParameters')[0].hide();

            // можно ли редактировать -> скрываем/показываем кнопку "Редактировать"
            var pointsAccess = App.UserInfo.getCurrentRole().AccessPoints;
            var access = pointsAccess.find(function (element) {
                return element.Resource == 'PromoSupports' && element.Action == 'PatchCostProduction';
            });

            customPromoSupportEditor.down('#editPromoSupportEditorButton').setVisible(access);
            customPromoSupportEditor.down('#savePromoSupportForm').setVisible(false);
            customPromoSupportEditor.down('#cancelPromoSupportForm').setVisible(false);

            promoLinkedGrid = associatedContainer.down('promolinkedcostprod').down('grid');
            customPromoSupportEditor.down("#PONumber").show();
        } else {
            promoLinkedGrid = associatedContainer.down('promolinkedticosts').down('grid');

            customPromoSupportEditor.down("#InvoiceNumber").show();
            customPromoSupportEditor.down('#costProductionFieldset').setVisible(false);

            // можно ли редактировать -> скрываем/показываем кнопку "Редактировать"
            var pointsAccess = App.UserInfo.getCurrentRole().AccessPoints;
            var access = pointsAccess.find(function (element) {
                return element.Resource == 'PromoSupports' && element.Action == 'Patch';
            });

            customPromoSupportEditor.down('#editPromoSupportEditorButton').setVisible(access); 
            customPromoSupportEditor.down('#savePromoSupportForm').setVisible(false);
            customPromoSupportEditor.down('#cancelPromoSupportForm').setVisible(false);
        }

        customPromoSupportEditor.width = '60%';
        //Иначе отобразиться только половинка окна
        customPromoSupportEditor.minWidth = 550;
        customPromoSupportEditor.down('#customPromoSupportEditorContainer').style = { borderLeft: 'none' };
        customPromoSupportEditor.singleUpdateMode = true;

        var promoSupportGrid = item.up('grid'),
            promoLinkedStore = promoLinkedGrid.getStore(),
            count = promoLinkedStore.getCount(),
            selModel = promoSupportGrid.getSelectionModel(),
            promoLinkedRecords = count > 0 ? promoLinkedStore.getRange(0, count) : [],
            promoLinkedViewerProxy = promoLinkedStore.getProxy();

        if (selModel.hasSelection()) {
            var selected = selModel.getSelection()[0];

            var promoLinkedIds = [];
            promoLinkedRecords.forEach(function (record) {
                promoLinkedIds.push(record.data.PromoId);
            });

            customPromoSupportEditor.promoSupportModel = selected;
            //customPromoSupportEditor.promoLinkedIds = promoLinkedIds;
            customPromoSupportEditor.PromoSupportPromoes = promoLinkedViewerProxy.getWriter().writeRecords(promoLinkedRecords, promoLinkedRecords.length).splice(0, promoLinkedRecords.length);
            this.fillSinglePromoSupportForm(customPromoSupportEditor);
        } else {
            App.Notify.pushInfo('No selection');
        }

        // кнопки добавить и удалить в promolinkedviewer
        var promoLinkedViewer = customPromoSupportEditor.down('promolinkedviewer');
        promoLinkedViewer.addListener('afterrender', function () {
            var toolbarpromoLinked = promoLinkedViewer.down('custombigtoolbar');
            toolbarpromoLinked.down('#addbutton').setDisabled(true);
            //toolbarpromoLinked.down('#updatebutton').setDisabled(true);
            toolbarpromoLinked.down('#deletebutton').setDisabled(true);
        });

        // кнопки прикрепления файла
        customPromoSupportEditor.down('#attachFile').setDisabled(true);
        customPromoSupportEditor.down('#deleteAttachFile').setDisabled(true);

        customPromoSupportEditor.show();
    },

    onEditPromoSupportEditorButton: function (button) {
        var customPromoSupportEditor = button.up('custompromosupporteditor');
        var isCostProduction = customPromoSupportEditor.down('#costProductionFieldset').isVisible();

        Ext.ComponentQuery.query('custompromosupporteditor field').forEach(function (field) {
            if (isCostProduction && field.needReadOnlyFromCostProduction === true) {
                field.setReadOnly(true);
                field.addCls('readOnlyField');
            }
            else {
                field.setReadOnly(false);
                field.removeCls('readOnlyField');
            }
        });

        // кнопки добавить и удалить в promolinkedviewer
        var promoLinkedViewer = customPromoSupportEditor.down('promolinkedviewer');
        var toolbarpromoLinked = promoLinkedViewer.down('custombigtoolbar');

        // Для Cost Production нельзя изменять список промо
        if (!isCostProduction) {
            toolbarpromoLinked.down('#addbutton').setDisabled(false);
            toolbarpromoLinked.down('#deletebutton').setDisabled(false);
        }
        if (promoLinkedViewer.down('directorygrid').getStore().totalCount === 0) {
            toolbarpromoLinked.down('#deletebutton').setDisabled(true);
        }

        //toolbarpromoLinked.down('#updatebutton').setDisabled(false);

        // кнопки прикрепления файла
        customPromoSupportEditor.down('#attachFile').setDisabled(false);
        customPromoSupportEditor.down('#deleteAttachFile').setDisabled(false);

        customPromoSupportEditor.down('#editPromoSupportEditorButton').setVisible(false);
        // кнопки сохранить и отменить
        customPromoSupportEditor.down('#savePromoSupportForm').setVisible(true);
        customPromoSupportEditor.down('#cancelPromoSupportForm').setVisible(true);

        // Off allocation можно изменить только при создании
        customPromoSupportEditor.down('checkboxfield[name=OffAllocationCheckbox]').setReadOnly(true);
    },

    onBeforeClosePromoSupportEditor: function (window) {
        var masterStore;
        if (Ext.ComponentQuery.query('promosupport')[0]) {
            masterStore = Ext.ComponentQuery.query('promosupport')[0].down('grid').getStore();
            Ext.ComponentQuery.query('promolinkedticosts')[0].down('grid').getStore().load();
        } else if (Ext.ComponentQuery.query('costproduction')[0]) {
            masterStore = Ext.ComponentQuery.query('costproduction')[0].down('grid').getStore();
            Ext.ComponentQuery.query('promolinkedcostprod')[0].down('grid').getStore().load();
        }

        if (masterStore) {
            masterStore.load();
        }        
    },

    // проверка того, чтобы суммы распределенные по промо не превышали общие на подстатью
    checkSummOfValues: function (editor) {
        var actualCostTEField = editor.down('[name=ActualCostTE]');
        var planCostTEField = editor.down('[name=PlanCostTE]');
        var planProductionCostField = editor.down('[name=PlanProductionCost]');
        var actualProductionCostField = editor.down('[name=ActualProductionCost]');
        var budgetItemName = editor.down('#promoSupportTypeField').getValue().toLowerCase();

        var promoLinkedViewer = editor.down('promolinkedviewer');
        var promoLinkedStore = promoLinkedViewer.down('grid').getStore(); 
        var data = promoLinkedStore.getProxy().data;

        var actualCostTESum = 0;
        var planCostTESum = 0;
        var planProductionCostSum = 0;
        var actualProductionCostSum = 0;
        data.forEach(function (item) {
            actualCostTESum += item.get('FactCalculation');
            planCostTESum += item.get('PlanCalculation');
            planProductionCostSum += item.get('PlanCostProd');
            actualProductionCostSum += item.get('FactCostProd');
        })

        var result = true;

        if (editor.costProduction) {
            if (planProductionCostSum > planProductionCostField.getValue()) {
                result = false;
                planProductionCostField.markInvalid(l10n.ns('tpm', 'PromoSupportPromo').value('ErrorSum'));
            }

            if (actualProductionCostSum > actualProductionCostField.getValue()) {
                result = false;
                actualProductionCostField.markInvalid(l10n.ns('tpm', 'PromoSupportPromo').value('ErrorSum'));
            }
        }
        else {
            if (actualCostTESum > actualCostTEField.getValue()) {
                result = false;
                actualCostTEField.markInvalid(l10n.ns('tpm', 'PromoSupportPromo').value('ErrorSum'));
            }

            // если не posm, то распределяется автоматически, не надо ограничений
            if (budgetItemName.indexOf('posm') >= 0 && planCostTESum > planCostTEField.getValue()) {
                result = false;
                planCostTEField.markInvalid(l10n.ns('tpm', 'PromoSupportPromo').value('ErrorSum'));
            }
        }

        this.checkPromoDates(editor);

        return result;
    },

    // проверка промо по датам
    checkPromoDates: function (editor) {
        var startDateField = editor.down('[name=StartDate]');
        var endDateField = editor.down('[name=EndDate]');
        var result = true;

        if (startDateField && endDateField && startDateField.getValue() && endDateField.getValue()) {
            var promoLinkedViewer = editor.down('promolinkedviewer');
            var promoLinkedStore = promoLinkedViewer.down('grid').getStore();
            var data = promoLinkedStore.getProxy().data;

            for (var i = 0; i < data.length && result; i++) {
                var startDatePromo = data[i].get('StartDate');
                var endDatePromo = data[i].get('EndDate');

                if (startDateField.getValue() >= endDatePromo || endDateField.getValue() <= startDatePromo) {
                    result = false;
                }
            }
        }

        return result;
    },

    // провести валидацию полей
    validateFields: function (editor) {
        var startDateValueField = editor.down('datefield[name=StartDate]');
        var endDateValueField = editor.down('datefield[name=EndDate]');
        var budgetSubItemField = editor.down('searchcombobox[name=BudgetSubItemId]');

        var fieldsToValidate = [startDateValueField, endDateValueField, budgetSubItemField];

        if (editor.costProduction) {
            var planProdCostValue = editor.down('numberfield[name=PlanProductionCost]');
            var actualProdCostValue = editor.down('numberfield[name=ActualProductionCost]');
            var poNumberValue = editor.down('textfield[name=PONumber]');

            fieldsToValidate.push(planProdCostValue);
            fieldsToValidate.push(actualProdCostValue);
            fieldsToValidate.push(poNumberValue);
        }
        else {
            var planQuantityValue = editor.down('numberfield[name=PlanQuantity]');
            var actualQuantityValue = editor.down('numberfield[name=ActualQuantity]');
            var planCostTEValue = editor.down('numberfield[name=PlanCostTE]');
            var actualCostTEValue = editor.down('numberfield[name=ActualCostTE]');
            var invoiceNumberValue = editor.down('textfield[name=InvoiceNumber]');

            fieldsToValidate.push(planQuantityValue);
            fieldsToValidate.push(actualQuantityValue);
            fieldsToValidate.push(planCostTEValue);
            fieldsToValidate.push(actualCostTEValue);
            fieldsToValidate.push(invoiceNumberValue);
        }

        var isValid = true;
        fieldsToValidate.forEach(function (n) {
            var fieldIsValid = n.isValid();
            if (!fieldIsValid) {
                n.validate();
                isValid = false;
            }
        })

        return isValid;    
    },

    // расширенный расширенный фильтр
    onFilterButtonClick: function (button) {
        var grid = this.getGridByButton(button);
        var store = grid.getStore();
        if (store.isExtendedStore) {
            var win = Ext.widget('extmasterfilter', store.getExtendedFilter());
            win.show();
        } else {
            console.error('Extended filter does not implemented for this store');
        }
    },
    
    onHistoryButtonClick: function (button) {
        var grid = this.getGridByButton(button),
            selModel = grid.getSelectionModel();

        if (selModel.hasSelection()) {
            var panel = grid.up('combineddirectorypanel'),
                model = panel.getBaseModel(),
                viewClassName = App.Util.buildViewClassName(panel, model, 'Historical');

            var baseReviewWindow = Ext.widget('basereviewwindow', { items: Ext.create(viewClassName, { baseModel: model }) });
            baseReviewWindow.show();

            var store = baseReviewWindow.down('grid').getStore();
            var proxy = store.getProxy();
            proxy.extraParams.Id = this.getRecordId(selModel.getSelection()[0]);
        }
    }
});