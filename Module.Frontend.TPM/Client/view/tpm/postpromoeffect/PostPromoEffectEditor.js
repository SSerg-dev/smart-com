Ext.define('App.view.tpm.postpromoeffect.PostPromoEffectEditor', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.postpromoeffecteditor',
    width: 800,
    minWidth: 800,
    maxHeight: 600,

    items: {
        xtype: 'editorform',
        columnsCount: 2,
        items: [{
            xtype: 'datefield',
            name: 'StartDate',
            fieldLabel: l10n.ns('tpm', 'PostPromoEffect').value('StartDate'),
            allowBlank: false,
            allowOnlyWhitespace: false,
            editable: false,
            format: 'd.m.Y',      
            listeners: {
                change: function (field, newValue, oldValue) {
                    if (newValue) {
                        var endDate = this.up('form').down('datefield[name=EndDate]');
                        endDate.getPicker().setMinDate(Ext.Date.add(newValue, Ext.Date.DAY, 1));
                        endDate.setMinValue(Ext.Date.add(newValue, Ext.Date.DAY, 1));
                    }
                }
            }
        }, {
            xtype: 'datefield',
            name: 'EndDate',
            fieldLabel: l10n.ns('tpm', 'PostPromoEffect').value('EndDate'),
            allowBlank: false,
            allowOnlyWhitespace: false,
            editable: false,
            format: 'd.m.Y',
            listeners: {
                change: function (field, newValue, oldValue) {
                    if (newValue) {
                        var startDate = this.up('form').down('datefield[name=StartDate]');
                        startDate.getPicker().setMaxDate(Ext.Date.add(newValue, Ext.Date.DAY, -1));
                        startDate.setMaxValue(Ext.Date.add(newValue, Ext.Date.DAY, -1));
                    }
                }
            }
        }, {
            xtype: 'treesearchfield',
            name: 'ClientTreeId',
            fieldLabel: l10n.ns('tpm', 'PostPromoEffect').value('ClientTreeFullPathName'),
            selectorWidget: 'clienttree',
            valueField: 'Id',
            displayField: 'FullPathName',
            store: {
                storeId: 'clienttreestore',
                model: 'App.model.tpm.clienttree.ClientTree',
                autoLoad: false,
                root: {}
            },
            mapping: [{
                from: 'FullPathName',
                to: 'ClientTreeFullPathName'
            }]
        }, {
            xtype: 'treesearchfield',
            name: 'ProductTreeId',
            fieldLabel: l10n.ns('tpm', 'PostPromoEffect').value('ProductTreeFullPathName'),
            selectorWidget: 'producttree',
            valueField: 'Id',
            displayField: 'FullPathName',
            store: {
                storeId: 'clienttreestore',
                model: 'App.model.tpm.producttree.ProductTree',
                autoLoad: false,
                root: {}
            },
            mapping: [{
                from: 'FullPathName',
                to: 'ProductTreeFullPathName'
            }]
        }, {
            xtype: 'numberfield',
            name: 'EffectWeek1',
            allowDecimals: true,
            allowExponential: false,
            minValue: -10000000000,
            maxValue: 10000000000,
            allowBlank: true,
            allowOnlyWhitespace: true,
            fieldLabel: l10n.ns('tpm', 'PostPromoEffect').value('EffectWeek1'),
        }, {
            xtype: 'numberfield',
            name: 'EffectWeek2',
            allowDecimals: true,
            allowExponential: false,
            minValue: -10000000000,
            maxValue: 10000000000,
            allowBlank: true,
            allowOnlyWhitespace: true,
            fieldLabel: l10n.ns('tpm', 'PostPromoEffect').value('EffectWeek2'),
        }, {
            xtype: 'numberfield',
            name: 'TotalEffect',
            allowDecimals: true,
            allowExponential: false,
            readOnly: true,
            cls: 'field-for-read-only',
            minValue: -10000000000,
            maxValue: 10000000000,
            allowBlank: true,
            allowOnlyWhitespace: true,
            fieldLabel: l10n.ns('tpm', 'PostPromoEffect').value('TotalEffect'),
        }]
    }
});
