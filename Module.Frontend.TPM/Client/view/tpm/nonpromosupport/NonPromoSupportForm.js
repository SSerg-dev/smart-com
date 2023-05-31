Ext.define('App.view.tpm.nonpromosupport.NonPromoSupportForm', {
    extend: 'Ext.panel.Panel',
	alias: 'widget.nonpromosupportform',

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
                    title: 'Equipment Type',
                    flex: 1,
                    margin: '0 0 0 5',
                    layout: 'fit',
                    items: [{
                        xtype: 'searchcombobox',
                        margin: '0 5 0 5',
						name: 'NonPromoEquipmentId',
						selectorWidget: 'nonpromoequipment',
                        valueField: 'Id',
						displayField: 'EquipmentType',
                        entityType: 'NonPromoEquipment',
                        allowBlank: false,
                        allowOnlyWhiteSpace: false,
                        store: {
                            type: 'simplestore',
                            autoLoad: false,
							model: 'App.model.tpm.nonpromoequipment.NonPromoEquipment',
                            extendedFilter: {
                                xclass: 'App.ExtFilterContext',
                                supportedModels: [{
                                    xclass: 'App.ExtSelectionFilterModel',
									model: 'App.model.tpm.nonpromoequipment.NonPromoEquipment',
                                    modelId: 'efselectionmodel'
                                }]
                            }
						},
						listeners: {
							change: function (field, newValue, oldValue) {
								if (newValue != oldValue) {
									var editor = field.up('customnonpromosupporteditor');
									if (editor) {
										editor.nonPromoEquipmentId = newValue;
									}
								}
							}
						}
                    }]
                }, {
                    xtype: 'fieldset',
                    id: 'InvoiceNumber',
                    title: l10n.ns('tpm', 'PromoSupport').value('InvoiceNumber'),
                    margin: '0 0 0 5',
                    flex: 2,
                    layout: 'fit',
                    items: [{
                        margin: '0 5 0 5',
                        xtype: 'textfield',
                        name: 'InvoiceNumber',
                        listeners: {
                            blur: function() {
                                this.setValue(this.getValue().trim());
                            }
                        },
                        maxLength: 50,
                        maxLengthText: l10n.ns('tpm', 'PromoSupport').value('InvoiceNumberMaxLengthText')
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
                    flex: 1,
                    padding: '0 5 5 5',
                    cls: 'promosupport-fieldset'
                },
                items: [{
                    xtype: 'numberfield',
                    name: 'PlanQuantity',
                    fieldLabel: 'Plan Quantity',
                }, {
                    xtype: 'numberfield',
                    name: 'ActualQuantity',
                    id: 'actualQuantityField',
                    fieldLabel: 'Actual Quantity',
                }, {
                    xtype: 'numberfield',
                    name: 'PlanCostTE',
                    fieldLabel: 'Plan Cost TE Total',
                }, {
                    xtype: 'numberfield',
                    name: 'ActualCostTE',
                    fieldLabel: 'Actual Cost TE Total',
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
                            width: '100%',
                            style: 'text-decoration: underline',
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
                    },
                    items: [{
                        xtype: 'datefield',
                        name: 'StartDate',
                        fieldLabel: 'Start Date',
                        editable: false,
                        allowBlank: false,
                        allowOnlyWhiteSpace: false,
                        listeners: {
                            change: function (field, newValue) {
                                if (newValue) {
                                    var endDateField = field.up('fieldset').down('[name=EndDate]');

                                    endDateField.clearInvalid();
                                    endDateField.setMinValue(Ext.Date.add(newValue, Ext.Date.DAY, 1));
                                    endDateField.setMaxValue(new Date(newValue.getFullYear(), 11, 31, 23, 59, 59));
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