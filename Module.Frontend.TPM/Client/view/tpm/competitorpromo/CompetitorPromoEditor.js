Ext.define('App.view.tpm.competitorpromo.CompetitorPromoEditor', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.competitorpromoeditor',
    width: 800,
    minWidth: 800,
    maxHeight: 600,
    cls: 'readOnlyFields',
    items: {
        xtype: 'editorform',
        items: [{
            xtype: 'searchfield',
            fieldLabel: l10n.ns('tpm', 'CompetitorPromo').value('CompetitorName'),
            name: 'CompetitorId',
            selectorWidget: 'competitor',
            valueField: 'Id',
            displayField: 'Name',
            store: {
                type: 'directorystore',
                model: 'App.model.tpm.competitor.Competitor',
                extendedFilter: {
                    xclass: 'App.ExtFilterContext',
                    supportedModels: [{
                        xclass: 'App.ExtSelectionFilterModel',
                        model: 'App.model.tpm.competitor.Competitor',
                        modelId: 'efselectionmodel'
                    }]
                }
            },
            listeners: {
                change: function (field, newValue, oldValue) {
                    var brandTechCombo = Ext.ComponentQuery.query('[name=CompetitorBrandTechId]')[0];
                    brandTechCombo.setValue('');
                    if (field.record != null) {
                        var value = field.record.get('Name');
                        if (brandTechCombo) {
                            var brandTechStore = brandTechCombo.getStore();
                            var filter = new Ext.util.Filter({
                                property: 'CompetitorId',
                                anyMatch: true,
                                operator: 'eq',
                                value: newValue,
                                disableOnEmpty: true
                            });
                            brandTechStore.clearFilter();
                            if (value != '') {
                                brandTechStore.addFilter(filter);
                            }
                        }
                    }
                },
            },
            mapping: [{
                from: 'Name',
                to: 'CompetitorName'
            }]
        }, {
            xtype: 'treesearchfield',
            name: 'ClientTreeObjectId',
            fieldLabel: l10n.ns('tpm', 'CompetitorPromo').value('ClientTreeFullPathName'),
            selectorWidget: 'clienttree',
            valueField: 'Id',
            displayField: 'FullPathName',
            clientTreeIdValid: true,
            store: {
                storeId: 'clienttreestore',
                model: 'App.model.tpm.clienttree.ClientTree',
                autoLoad: false,
                root: {}
            },
            listeners:
            {
                change: function (field, newValue, oldValue) {
                    if (field && field.record && field.record.data.ObjectId === 5000000) {
                        this.clientTreeIdValid = false;
                    } else {
                        this.clientTreeIdValid = true;
                    }
                }
            },
            validator: function () {
                if (!this.clientTreeIdValid) {
                    return l10n.ns('core', 'customValidators').value('clientTreeSelectRoot')
                }
                return true;
            },
            mapping: [{
                from: 'FullPathName',
                to: 'ClientTreeFullPathName'
            }]
        }, {
            xtype: 'searchfield',
            fieldLabel: l10n.ns('tpm', 'Promo').value('BrandTechName'),
            name: 'CompetitorBrandTechId',
            selectorWidget: 'competitorbrandtech',
            valueField: 'Id',
            displayField: 'BrandTech',
            store: {
                type: 'directorystore',
                model: 'App.model.tpm.competitorbrandtech.CompetitorBrandTech',
                extendedFilter: {
                    xclass: 'App.ExtFilterContext',
                    supportedModels: [{
                        xclass: 'App.ExtSelectionFilterModel',
                        model: 'App.model.tpm.competitorbrandtech.CompetitorBrandTech',
                        modelId: 'efselectionmodel'
                    }, {
                        xclass: 'App.ExtTextFilterModel',
                        modelId: 'eftextmodel'
                    }]
                },
                listeners: {
                    load: function (store, operation, eOpts) {
                        var brandTechCombo = Ext.ComponentQuery.query('[name=CompetitorBrandTechId]')[0];
                        var competitorField = Ext.ComponentQuery.query('[name=CompetitorId]')[0];
                        if (brandTechCombo.filtered == true) return;
                        if (competitorField.rawValue != null) {
                            var value = competitorField.rawValue;
                            if (brandTechCombo) {
                                var filter = new Ext.util.Filter({
                                    property: 'CompetitorName',
                                    anyMatch: true,
                                    operator: 'like',
                                    value: value,
                                    disableOnEmpty: true
                                });
                                store.clearFilter();
                                if (value != '') {
                                    brandTechCombo.filtered = true;
                                    store.addFilter(filter);
                                }
                            }
                        }
                    }
                }
            },
            mapping: [{
                from: 'BrandTech',
                to: 'CompetitorBrandTechName'
            }]
        }, {
            xtype: 'textfield',
            fieldLabel: l10n.ns('tpm', 'Promo').value('Name'),
            name: 'Name',
        }, {
            xtype: 'datefield',
            fieldLabel: l10n.ns('tpm', 'Promo').value('StartDate'),
            name: 'StartDate',
            listeners:
            {
                change: function (field, newValue, oldValue) {
                    this.up('form').down('[name=EndDate]').setMinValue(newValue);
                }
            },
            validator: function () {
                return true;
            },
        }, {
            xtype: 'datefield',
            fieldLabel: l10n.ns('tpm', 'Promo').value('EndDate'),
            name: 'EndDate',
            listeners:
            {
                change: function (field, newValue, oldValue) {
                    this.up('form').down('[name=StartDate]').setMaxValue(newValue);
                }
            },
            validator: function () {
                return true;
            },
        }, {
            xtype: 'textfield',
            fieldLabel: l10n.ns('tpm', 'CompetitorPromo').value('Price'),
            name: 'Price',
            allowBlank: true,
            allowOnlyWhitespace: true,
            validator: function (value) {
                return new Number(value) > 0;
            },
        }, {
            xtype: 'textfield',
            fieldLabel: l10n.ns('tpm', 'CompetitorPromo').value('Discount'),
            name: 'Discount',
            allowBlank: true,
            allowOnlyWhitespace: true,
            validator: function (value) {
                return new Number(value) > 0;
            },
        }
        ]
    }
});
