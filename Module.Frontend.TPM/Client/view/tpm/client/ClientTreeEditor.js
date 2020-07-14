Ext.define('App.view.tpm.client.ClientTreeEditor', {
    extend: 'App.view.core.common.EditorWindow',
    alias: 'widget.clienttreeeditor',
    width: 500,
    minWidth: 500,
    maxHeight: 550,

    items: {
        xtype: 'editorform',
        columnsCount: 1,
        items: [{
            xtype: 'searchcombobox',
            fieldLabel: l10n.ns('tpm', 'NodeType').value('Name'),
            name: 'Type',
            selectorWidget: 'nodetype',
            valueField: 'Name',
            displayField: 'Name',
            entityType: 'NodeType',
            store: {
                type: 'simplestore',
                model: 'App.model.tpm.nodetype.NodeType',
                extendedFilter: {
                    xclass: 'App.ExtFilterContext',
                    supportedModels: [{
                        xclass: 'App.ExtSelectionFilterModel',
                        model: 'App.model.tpm.nodetype.NodeType',
                        modelId: 'efselectionmodel'
                    }]
                },
                fixedFilters: {
                    'typefilter': {
                        property: 'Type',
                        operation: 'Equals',
                        value: 'CLIENT'
                    }
                }
            },
            mapping: [{
                from: 'Name',
                to: 'Type'
            }]
        }, {
            xtype: 'textfield',
            fieldLabel: l10n.ns('tpm', 'ClientTree').value('Name'),
            name: 'Name',
        }, {
            xtype: 'textfield',
            fieldLabel: l10n.ns('tpm', 'ClientTree').value('GHierarchyCode'),
            name: 'GHierarchyCode',
            allowBlank: true,
            allowOnlyWhitespace: true,
        }, {
            xtype: 'textfield',
            fieldLabel: l10n.ns('tpm', 'ClientTree').value('DemandCode'),
            name: 'DemandCode',
            allowBlank: true,
            allowOnlyWhitespace: true,
        }, {
            xtype: 'booleancombobox',
            fieldLabel: l10n.ns('tpm', 'ClientTree').value('IsBaseClient'),
            name: 'IsBaseClient',
            store: {
                type: 'booleanstore',
                data: [
                    { id: true, text: l10n.ns('tpm', 'booleanValues').value('true') },
                    { id: false, text: l10n.ns('tpm', 'booleanValues').value('false') }
                ]
            },
            allowBlank: false,
            allowOnlyWhitespace: false,
            listeners: {
                change: function (booleancombobox, newValue, oldValue) {
                    if (!Ext.ComponentQuery.query('clienttree')[0].down('clienttreegrid').createMode && newValue != undefined && oldValue != undefined) {
                        Ext.Msg.show({
                            title: l10n.ns('core').value('alertTitle'),
                            msg: l10n.ns('tpm', 'ClientTree').value('WarningChangeIsBase'),
                            buttons: Ext.MessageBox.OK,
                            icon: Ext.Msg.INFO
                        });
                    }
                    var isOnInvoiceCombobox = this.up('container').down('combobox[name=IsOnInvoice]');
                    var deviationCoefficient = this.up('container').down('sliderfield[name=DeviationCoefficient]');
                    var adjustment = this.up('container').down('numberfield[name=Adjustment]');
                    if (newValue) {
                        isOnInvoiceCombobox.setDisabled(false);

                        deviationCoefficient.setDisabled(false);
                        adjustment.setDisabled(false);
                    } else {
                        isOnInvoiceCombobox.setDisabled(true);
                        isOnInvoiceCombobox.setValue('');

                        adjustment.setDisabled(true);
                        adjustment.setValue(0);
                        deviationCoefficient.setDisabled(true);
                        deviationCoefficient.setValue(0);
                    }
                }
            }
        }, {
            xtype: 'booleancombobox',
            fieldLabel: l10n.ns('tpm', 'ClientTree').value('InvoiceType'),
            name: 'IsOnInvoice',
            editable: false,
            store: {
                type: 'booleanstore',
                fields: ['id', 'text'],
                data: [
                    { id: null, text: '\u00a0' },
                    { id: true, text: l10n.ns('tpm', 'InvoiceTypes').value('OnInvoice') },
                    { id: false, text: l10n.ns('tpm', 'InvoiceTypes').value('OffInvoice') }
                ]
            },
        }, {
            xtype: 'textfield',
            name: 'DMDGroup',
            allowBlank: true,
            allowOnlyWhitespace: true,
            regex: /^[0-9]*[0-9]$/,
            fieldLabel: l10n.ns('tpm', 'ClientTree').value('DMDGroup'),
            regexText: l10n.ns('tpm', 'ClientTree').value('DMDGroupRegex'),

        }, {
            xtype: 'searchcombobox',
            fieldLabel: l10n.ns('tpm', 'RetailType').value('Name'),
            name: 'RetailTypeName',
            selectorWidget: 'retailtype',
            valueField: 'Name',
            displayField: 'Name',
            entityType: 'RetailType',
            allowBlank: true,
            allowOnlyWhitespace: true,
            store: {
                type: 'simplestore',
                model: 'App.model.tpm.retailtype.RetailType',
                extendedFilter: {
                    xclass: 'App.ExtFilterContext',
                    supportedModels: [{
                        xclass: 'App.ExtSelectionFilterModel',
                        model: 'App.model.tpm.retailtype.RetailType',
                        modelId: 'efselectionmodel'
                    }]
                }
            },
            mapping: [{
                from: 'Name',
                to: 'RetailTypeName'
            }]
        }, {
            xtype: 'numberfield',
            fieldLabel: l10n.ns('tpm', 'ClientTree').value('DistrMarkUp'),
            name: 'DistrMarkUp',
            allowBlank: false,
            allowDecimals: true,
            allowOnlyWhitespace: false,
            allowExponential: false,
            regex: /^\d+\,?\d*$/,
            regexText: l10n.ns('tpm', 'ClientTree').value('DMDGroupRegex'),
            validator: function() {
                value = this.getValue();
                if (value === 0) {
                    return 'The value "0" is not allowed';
                }
                if (value < 0) {
                    return 'Negative values are not allowed';
                }
                return true;
            }
        }, {
            xtype: 'dispatchdate',
            itemId: 'dispatchClientTreeEditor'
        }, {
            xtype: 'numberfield',
            fieldLabel: l10n.ns('tpm', 'ClientTree').value('PostPromoEffectW1'),
            name: 'PostPromoEffectW1',
            allowBlank: true,
            allowDecimals: true,
            allowOnlyWhitespace: true,
            allowExponential: false,
            maxValue: 100000,
            minValue: -100000,
        }, {
            xtype: 'numberfield',
            fieldLabel: l10n.ns('tpm', 'ClientTree').value('PostPromoEffectW2'),
            name: 'PostPromoEffectW2',
            allowBlank: true,
            allowDecimals: true,
            allowOnlyWhitespace: true,
            allowExponential: false,
            maxValue: 100000,
            minValue: -100000,
        }]
    }
});
