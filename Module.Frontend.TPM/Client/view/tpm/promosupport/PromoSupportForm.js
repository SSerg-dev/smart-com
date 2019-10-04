Ext.define('App.view.tpm.promosupport.PromoSupportForm', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.promosupportform',

    items: [{
        xtype: 'custompromopanel',
        style: 'padding: 0 !important;',
        minHeight: 0,
        margin: 1,
        layout: {
            type: 'vbox',
            align: 'stretch'
        },
        items: [{
            xtype: 'container',
            padding: '0 10 0 10',
            defaults: {
                margin: 0,
            },
            items: [{
                xtype: 'container',
                layout: {
                    type: 'hbox',
                    align: 'stretch'
                },
                defaults: {
                    cls: 'promosupport-fieldset',
                },
                items: [{
                    xtype: 'fieldset',
                    title: 'Promo Support Type',
                    flex: 1,
                    margin: '0 5 0 0',
                    layout: 'fit',
                    items: [{
                        xtype: 'singlelinedisplayfield',
                        itemId: 'promoSupportTypeField',
                        needClear: true,
                        margin: '0 5 0 5',
                        needReadOnlyFromCostProduction: true,
                    }]
                }, {
                    xtype: 'fieldset',
                    title: 'Equipment Type',
                    flex: 1,
                    margin: '0 0 0 5',
                    layout: 'fit',
                    items: [{
                        xtype: 'searchcombobox',
                        margin: '0 5 0 5',
                        name: 'BudgetSubItemId',
                        selectorWidget: 'budgetsubitem',
                        valueField: 'Id',
                        displayField: 'Name',
                        entityType: 'BudgetSubItem',
                        allowBlank: false,
                        allowOnlyWhiteSpace: false,
                        needClear: true,
                        needReadOnlyFromCostProduction: true,
                        store: {
                            type: 'simplestore',
                            autoLoad: false,
                            model: 'App.model.tpm.budgetsubitem.BudgetSubItemWithFilter',
                            extendedFilter: {
                                xclass: 'App.ExtFilterContext',
                                supportedModels: [{
                                    xclass: 'App.ExtSelectionFilterModel',
                                    model: 'App.model.tpm.budgetsubitem.BudgetSubItemWithFilter',
                                    modelId: 'efselectionmodel'
                                }]
                            }
                        },
                        listeners: {
                            change: function (field, newValue, oldValue) {
                                if (newValue) {
                                    onChangeFieldEvent(field);
                                }
                            }
                        }
                    }]
                }, {
                    xtype: 'fieldset',
                    id: 'PONumber',
                    title: l10n.ns('tpm', 'PromoSupport').value('PONumber'),
                    margin: '0 0 0 5',
                    flex: 2,
                    layout: 'fit',
                    hidden: true,
                    items: [{
                        margin: '0 5 0 5',
                        xtype: 'textfield',
                        name: 'PONumber',
                        regex: /^[0-9]*[0-9]$/,
                        regexText: l10n.ns('tpm', 'PromoSupport').value('PONumberRegex'),
                    }]
                }, {
                    xtype: 'fieldset',
                    id: 'InvoiceNumber',
                    title: l10n.ns('tpm', 'PromoSupport').value('InvoiceNumber'),
                    margin: '0 0 0 5',
                    flex: 2,
                    layout: 'fit',
                    hidden: true,
                    items: [{
                        margin: '0 5 0 5',
                        xtype: 'textfield',
                        name: 'InvoiceNumber',
                        regex: /^[0-9]*[0-9]$/,
                        regexText: l10n.ns('tpm', 'PromoSupport').value('InvoiceNumberRegex'),
                    }]
                }]
            }, {
                xtype: 'fieldset',
                title: 'Parameters',
                itemId: 'promoSupportFormParameters',
                layout: {
                    type: 'hbox',
                    align: 'stretch'
                },
                defaults: {
                    minValue: 0,
                    maxValue: 2000000000,
                    labelAlign: 'top',
                    hideTrigger: true,
                    needClear: true,
                    flex: 1,
                    padding: '0 5 5 5',
                    cls: 'promosupport-fieldset',
                    listeners: {
                        change: function (field, newValue, oldValue) {
                            if (newValue) {
                                onChangeFieldEvent(field);
                            }
                        }
                    }
                },
                items: [{
                    xtype: 'numberfield',
                    name: 'PlanQuantity',
                    fieldLabel: 'Plan Quantity',
                    needReadOnlyFromCostProduction: true,
                    listeners: {
                        change: function (field) {
                            var customPromoPanel = field.up('custompromopanel');
                            var planProductionCost = customPromoPanel.down('[name=PlanProductionCost]');
                            var planProdCostPer1Item = customPromoPanel.down('[name=PlanProdCostPer1Item]');
                            var planProductionCostNewValue = planProdCostPer1Item.getValue() * field.getValue();
                            var planQuantityCopy = field.up('custompromopanel').down('[name=PlanQuantityCopy]');

                            planProductionCost.setValue(planProductionCostNewValue);
                            planQuantityCopy.setValue(field.getValue());
                        }
                    }
                }, {
                    xtype: 'numberfield',
                    name: 'ActualQuantity',
                    fieldLabel: 'Actual Quantity',
                    needReadOnlyFromCostProduction: true,
                    listeners: {
                        change: function (field) {
                            var customPromoPanel = field.up('custompromopanel');
                            var actualProductionCost = customPromoPanel.down('[name=ActualProductionCost]');
                            var actualProdCostPer1Item = customPromoPanel.down('[name=ActualProdCostPer1Item]');
                            var actualProductionCostNewValue = actualProdCostPer1Item.getValue() * field.getValue();
                            var actualQuantityCopy = field.up('custompromopanel').down('[name=ActualQuantityCopy]');

                            actualProductionCost.setValue(actualProductionCostNewValue);
                            actualQuantityCopy.setValue(field.getValue());
                        }
                    }
                }, {
                    xtype: 'numberfield',
                    name: 'PlanCostTE',
                    fieldLabel: 'Plan Cost TE Total',
                    needReadOnlyFromCostProduction: true,
                }, {
                    xtype: 'numberfield',
                    name: 'ActualCostTE',
                    fieldLabel: 'Actual Cost TE Total',
                    needReadOnlyFromCostProduction: true,
                }]
            }, {
                xtype: 'fieldset',
                title: 'Cost Production',
                itemId: 'costProductionFieldset',
                layout: {
                    type: 'vbox',
                    align: 'stretch'
                },
                items: [{
                    xtype: 'container',
                    layout: {
                        type: 'hbox',
                        align: 'stretch'
                    },
                    defaults: {
                        labelAlign: 'top',
                        flex: 1,
                        padding: '0 5 5 5',
                        cls: 'promosupport-fieldset',
                        hideTrigger: true,
                        needClear: true,
                        listeners: {
                            change: function (field, newValue, oldValue) {
                                if (newValue) {
                                    onChangeFieldEvent(field);
                                }
                            }
                        }
                    },
                    items: [{
                        xtype: 'numberfield',
                        name: 'PlanProdCostPer1Item',
                        fieldLabel: 'Plan prod cost per 1 item',
                        needReadOnlyFromCostProduction: false,
                        minValue: 0,
                        maxValue: 2000000000,
                        allowDecimal: true,
                        listeners: {
                            change: function (field) {
                                var customPromoPanel = field.up('custompromopanel');
                                var planProductionCost = customPromoPanel.down('[name=PlanProductionCost]');
                                var planQuantity = customPromoPanel.down('[name=PlanQuantity]');
                                var planProductionCostNewValue = planQuantity.getValue() * field.getValue();

                                planProductionCost.setValue(planProductionCostNewValue);
                                planProductionCost.clearInvalid();
                            }
                        }
                    }, {
                        xtype: 'numberfield',
                        name: 'ActualProdCostPer1Item',
                        fieldLabel: 'Actual prod cost per 1 item',
                        needReadOnlyFromCostProduction: false,                        
                        minValue: 0,
                        maxValue: 2000000000,
                        allowDecimal: true,
                        listeners: {
                            change: function (field) {
                                var customPromoPanel = field.up('custompromopanel');
                                var actualProductionCost = customPromoPanel.down('[name=ActualProductionCost]');
                                var actualQuantity = customPromoPanel.down('[name=ActualQuantity]');
                                var actualProductionCostNewValue = actualQuantity.getValue() * field.getValue();

                                actualProductionCost.setValue(actualProductionCostNewValue);
                                actualProductionCost.clearInvalid();
                            }
                        }
                    }]
                }, {
                    xtype: 'container',
                    layout: {
                        type: 'hbox',
                        align: 'stretch'
                    },
                    defaults: {
                        labelAlign: 'top',
                        flex: 1,
                        padding: '0 5 5 5',
                        cls: 'promosupport-fieldset',
                        hideTrigger: true,
                        needClear: true,
                        listeners: {
                            change: function (field, newValue, oldValue) {
                                if (newValue) {
                                    onChangeFieldEvent(field);
                                }
                            }
                        }
                    },
                    items: [{
                        xtype: 'numberfield',
                        name: 'PlanQuantityCopy',
                        fieldLabel: 'Plan Quantity',
                        needReadOnlyFromCostProduction: true,                        
                    }, {
                        xtype: 'numberfield',
                        name: 'PlanProductionCost',
                        fieldLabel: 'Plan production cost',
                        needReadOnlyFromCostProduction: true,                        
                    }, {
                        xtype: 'numberfield',
                        name: 'ActualQuantityCopy',
                        fieldLabel: 'Actual Quantity',
                        needReadOnlyFromCostProduction: true,
                    }, {
                        xtype: 'numberfield',
                        name: 'ActualProductionCost',
                        fieldLabel: 'Actual production cost',
                        needReadOnlyFromCostProduction: true,
                    }]
                }]
            }, {
                xtype: 'container',
                layout: {
                    type: 'hbox',
                    align: 'stretch'
                },
                items: [{
                    xtype: 'fieldset',
                    title: 'Attach File',
                    flex: 1,
                    padding: '0 8 10 8',
                    margin: '0 5 5 0',
                    layout: 'hbox',
                    defaults: {
                        cls: 'promosupport-fieldset',
                    },
                    items: [{
                        xtype: 'container',
                        flex: 5,
                        layout: {
                            type: 'vbox',
                        },
                        items: [{
                            xtype: 'label',
                            text: 'FileName: ',
                            margin: '5 0 0 0',
                            style: 'color: #455a64'
                        }, {
                            xtype: 'singlelinedisplayfield',
                            itemId: 'attachFileName',
                            readOnly: true,
                            value: 'Attach your file',
                            needReadOnlyFromCostProduction: true,
                            width: '100%',
                            style: 'text-decoration: underline',
                            needClear: true,
                        }]
                    }, {
                        xtype: 'button',
                        itemId: 'attachFile',
                        text: 'Attach File',
                        cls: 'attachfile-button',
                        flex: 1,
                        minWidth: 85,
                        margin: '21 4 0 4'
                    }, {
                        xtype: 'button',
                        itemId: 'deleteAttachFile',
                        text: 'Delete',
                        cls: 'deletefile-button',
                        flex: 1,
                        minWidth: 60,
                        margin: '21 0 0 0'
                    }]
                }, {
                    xtype: 'fieldset',
                    title: 'Period',
                    margin: '0 0 5 5',
                    flex: 1,
                    layout: {
                        type: 'hbox',
                        align: 'stretch'
                    },
                    defaults: {
                        labelAlign: 'top',
                        flex: 1,
                        padding: '0 5 5 5',
                        cls: 'promosupport-fieldset',
                        needClear: true,
                        listeners: {
                            change: function (field, newValue, oldValue) {
                                if (newValue) {
                                    onChangeFieldEvent(field);
                                }
                            }
                        }
                    },
                    items: [{
                        xtype: 'datefield',
                        name: 'StartDate',
                        fieldLabel: 'Start Date',
                        editable: false,
                        needReadOnlyFromCostProduction: true,
                        allowBlank: false,
                        allowOnlyWhiteSpace: false,
                        listeners: {
                            change: function (field, newValue) {
                                if (newValue) {
                                    var endDateField = field.up('fieldset').down('[name=EndDate]');

                                    endDateField.clearInvalid();
                                    endDateField.setMinValue(Ext.Date.add(newValue, Ext.Date.DAY, 1));
                                    endDateField.getPicker().setMinDate();

                                    if (endDateField.getValue())
                                        endDateField.validate();

                                    field.setMaxValue();
                                    field.getPicker().setMaxDate();
                                    field.clearInvalid();
                                }
                            }
                        }
                    }, {
                        xtype: 'datefield',
                        name: 'EndDate',
                        fieldLabel: 'End Date',
                        editable: false,
                        needReadOnlyFromCostProduction: true,
                        allowBlank: false,
                        allowOnlyWhiteSpace: false,
                        listeners: {
                            change: function (field, newValue) {
                                if (newValue) {
                                    var startDateField = field.up('fieldset').down('[name=StartDate]');

                                    startDateField.clearInvalid();
                                    startDateField.setMaxValue(Ext.Date.add(newValue, Ext.Date.DAY, -1));
                                    startDateField.getPicker().setMaxDate();

                                    if (startDateField.getValue())
                                        startDateField.validate();

                                    field.setMinValue();
                                    field.getPicker().setMinDate();
                                    field.clearInvalid();
                                }
                            }
                        }
                    }]
                }]
            }]
        }]
    }]
});

function onChangeFieldEvent(field) {
    var editor = field.up('custompromosupporteditor'),
        mainContainer = editor.down('#mainPromoSupportLeftToolbarContainer'),
        selectedItem;

    mainContainer.items.items.forEach(function (item) {
        if (item.hasCls('selected')) {
            selectedItemId = item.id;
            selectedItem = item;
        }
    });

    //TODO: требуется доработка при изменении сохраненной записи
    //if (selectedItem) {
    //    selectedItem.saved = false;
    //}

    //createButton = editor.down('#createPromoSupport').setDisabled(true);
    //createOnTheBasisButton = editor.down('#createPromoSupportOnTheBasis').setDisabled(true);
}