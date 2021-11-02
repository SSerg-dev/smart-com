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
            xtype: 'textfield',
            fieldLabel: l10n.ns('tpm', 'Promo').value('Number'),
            name: 'Number',
            readonly: true
        }, {
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
            mapping: [{
                from: 'Name',
                to: 'CompetitorName'
            }]
        }, {
            xtype: 'treesearchfield',
            name: 'ClientTreeId',
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
        }, {
            xtype: 'datefield',
            fieldLabel: l10n.ns('tpm', 'Promo').value('EndDate'),
            name: 'EndDate',
        }, {
            xtype: 'textfield',
            fieldLabel: l10n.ns('tpm', 'CompetitorPromo').value('Price'),
            name: 'Price',
        }, {
            xtype: 'textfield',
            fieldLabel: l10n.ns('tpm', 'CompetitorPromo').value('Discount'),
            name: 'Discount',
        }, {
            xtype: 'textfield',
            fieldLabel: l10n.ns('tpm', 'CompetitorPromo').value('Subrange'),
            name: 'Subrange',
        }, {
            xtype: 'searchfield',
            fieldLabel: l10n.ns('tpm', 'Promo').value('PromoStatusName'),
            name: 'PromoStatusId',
            selectorWidget: 'promostatus',
            valueField: 'Id',
            displayField: 'Name',
            store: {
                type: 'directorystore',
                model: 'App.model.tpm.promostatus.PromoStatus',
                extendedFilter: {
                    xclass: 'App.ExtFilterContext',
                    supportedModels: [{
                        xclass: 'App.ExtSelectionFilterModel',
                        model: 'App.model.tpm.promostatus.PromoStatus',
                        modelId: 'efselectionmodel'
                    }, {
                        xclass: 'App.ExtTextFilterModel',
                        modelId: 'eftextmodel'
                    }]
                }
            },
            mapping: [{
                from: 'Name',
                to: 'Status'
            }]
        }
        ]
    }
});
