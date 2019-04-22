﻿Ext.define('App.view.tpm.client.ClientTreeEditor', {
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
            fieldLabel: l10n.ns('tpm', 'ClientTree').value('ExecutionCode'),
            name: 'ExecutionCode',
            allowBlank: false,
            allowOnlyWhitespace: false,
        }, {
            xtype: 'textfield',
            fieldLabel: l10n.ns('tpm', 'ClientTree').value('DemandCode'),
            name: 'DemandCode',
            allowBlank: true,
            allowOnlyWhitespace: true,
        }, {
            xtype: 'numberfield',
            fieldLabel: l10n.ns('tpm', 'ClientTree').value('Share'),
            name: 'Share',
            maxValue: 100,
            minValue: 0,
            step: 1,
            allowBlank: false,
            allowOnlyWhitespace: false,
            allowDecimals: false
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
