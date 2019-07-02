Ext.define('App.view.tpm.incrementalpromo.IncrementalPromoEditor', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.incrementalpromoeditor',
    width: 800,
    minWidth: 800,
    maxHeight: 600,
    cls: 'readOnlyFields',

    items: {
        xtype: 'editorform',
        columnsCount: 2,
        items: [{
            xtype: 'searchfield',
            name: 'PromoId',
            fieldLabel: l10n.ns('tpm', 'IncrementalPromo').value('PromoId'),
            selectorWidget: 'promo',
            valueField: 'Id',
            displayField: 'Number',
            store: {
                type: 'directorystore',
                model: 'App.model.tpm.promo.Promo',
                extendedFilter: {
                    xclass: 'App.ExtFilterContext',
                    supportedModels: [{
                        xclass: 'App.ExtSelectionFilterModel',
                        model: 'App.model.tpm.promo.Promo',
                        modelId: 'efselectionmodel'
                    }]
                }
            },
            mapping: [{
                from: 'Number',
                to: 'PromoNumber'
            }],
            onTrigger2Click: function () {
                this.reset();
                this.up('editorform').down('[name=PromoName]').reset();
                this.up('editorform').down('[name=PromoBrandTechName]').reset();
                this.up('editorform').down('[name=PromoDispatchesStart]').reset();
                this.up('editorform').down('[name=PromoDispatchesEnd]').reset();
                this.up('editorform').down('[name=PromoStartDate]').reset();
                this.up('editorform').down('[name=PromoEndDate]').reset();
            },
            listeners: {
                change: function (searchfield) {
                    if (searchfield.getRecord()) {
                        searchfield.up('editorform').down('[name=PromoName]').setValue(searchfield.getRecord().data.Name);
                        searchfield.up('editorform').down('[name=PromoBrandTechName]').setValue(searchfield.getRecord().data.BrandTechName);
                        searchfield.up('editorform').down('[name=PromoDispatchesStart]').setValue(searchfield.getRecord().data.DispatchesStart);
                        searchfield.up('editorform').down('[name=PromoDispatchesEnd]').setValue(searchfield.getRecord().data.DispatchesEnd);
                        searchfield.up('editorform').down('[name=PromoStartDate]').setValue(searchfield.getRecord().data.StartDate);
                        searchfield.up('editorform').down('[name=PromoEndDate]').setValue(searchfield.getRecord().data.EndDate);
                    }
                }
            }
        }, {
            xtype: 'textfield',
            name: 'PromoName',
            fieldLabel: l10n.ns('tpm', 'IncrementalPromo').value('PromoName'),
            readOnlyCls: 'readOnlyField',
            readOnly: true,
            setReadOnly: function () { return false },
        }, {
            xtype: 'textfield',
            name: 'PromoBrandTechName',
            fieldLabel: l10n.ns('tpm', 'IncrementalPromo').value('PromoBrandTechName'),
            readOnlyCls: 'readOnlyField',
            readOnly: true,
            setReadOnly: function () { return false },
        }, {
            xtype: 'datefield',
            name: 'PromoStartDate',
            fieldLabel: l10n.ns('tpm', 'IncrementalPromo').value('PromoStartDate'),
            minValue: new Date(),
            allowBlank: false,
            editable: false,
            format: 'd.m.Y',
            readOnlyCls: 'readOnlyField',
            readOnly: true,
            setReadOnly: function () { return false },
        }, {
            xtype: 'datefield',
            name: 'PromoEndDate',
            fieldLabel: l10n.ns('tpm', 'IncrementalPromo').value('PromoEndDate'),
            minValue: new Date(),
            allowBlank: false,
            editable: false,
            format: 'd.m.Y',
            readOnlyCls: 'readOnlyField',
            readOnly: true,
            setReadOnly: function () { return false },
        }, {
            xtype: 'datefield',
            name: 'PromoDispatchesStart',
            fieldLabel: l10n.ns('tpm', 'IncrementalPromo').value('PromoDispatchesStart'),
            minValue: new Date(),
            allowBlank: false,
            editable: false,
            format: 'd.m.Y',
            readOnlyCls: 'readOnlyField',
            readOnly: true,
            setReadOnly: function () { return false },
        }, {
            xtype: 'datefield',
            name: 'PromoDispatchesEnd',
            fieldLabel: l10n.ns('tpm', 'IncrementalPromo').value('PromoDispatchesEnd'),
            minValue: new Date(),
            allowBlank: false,
            editable: false,
            format: 'd.m.Y',
            readOnlyCls: 'readOnlyField',
            readOnly: true,
            setReadOnly: function () { return false },
        }, {
            xtype: 'searchfield',
            name: 'ProductId',
            fieldLabel: l10n.ns('tpm', 'IncrementalPromo').value('ProductZREP'),
            selectorWidget: 'product',
            valueField: 'Id',
            displayField: 'ZREP',
            store: {
                type: 'directorystore',
                model: 'App.model.tpm.product.Product',
                extendedFilter: {
                    xclass: 'App.ExtFilterContext',
                    supportedModels: [{
                        xclass: 'App.ExtSelectionFilterModel',
                        model: 'App.model.tpm.product.Product',
                        modelId: 'efselectionmodel'
                    }]
                }
            },
            mapping: [{
                from: 'ZREP',
                to: 'ProductZREP'
            }],
        }, {
            xtype: 'numberfield',
            name: 'IncrementalCaseAmount',
            fieldLabel: l10n.ns('tpm', 'IncrementalPromo').value('IncrementalCaseAmount'),
            allowBlank: false,
        }, {
            xtype: 'numberfield',
            name: 'IncrementalLSV',
            fieldLabel: l10n.ns('tpm', 'IncrementalPromo').value('IncrementalLSV'),
            allowBlank: false,
        }, {
            xtype: 'numberfield',
            name: 'IncrementalPrice',
            fieldLabel: l10n.ns('tpm', 'IncrementalPromo').value('IncrementalPrice'),
            allowBlank: false,
        }]
    }
});
