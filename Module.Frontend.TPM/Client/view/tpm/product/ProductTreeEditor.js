Ext.define('App.view.tpm.product.ProductTreeEditor', {
    extend: 'App.view.core.common.EditorWindow',
    alias: 'widget.producttreeeditor',
    width: 500,
    minWidth: 500,
    maxHeight: 500,

    listeners:{
        afterrender: function (window) {
            window.down('searchcombobox[name=BrandId]').hide();
            window.down('searchcombobox[name=TechnologyId]').hide();
        }
    },

    items: [{
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
                        value: 'PRODUCT'
                    }
                }
            },
            listeners: {
                change: function (field, newValue, oldValue) {
                    if (newValue === 'Brand') {
                        field.up('editorform').down('searchcombobox[name=BrandId]').show();
                        field.up('editorform').down('searchcombobox[name=TechnologyId]').hide();
                        field.up('editorform').down('textfield[name=Name]').hide();
                        field.up('editorform').down('textfield[name=Abbreviation]').setDisabled(false);
                    } else if (newValue === 'Technology') {
                        field.up('editorform').down('searchcombobox[name=BrandId]').hide();
                        field.up('editorform').down('searchcombobox[name=TechnologyId]').show();
                        field.up('editorform').down('textfield[name=Name]').hide();
                        field.up('editorform').down('textfield[name=Abbreviation]').setDisabled(false);
                    } else {
                        field.up('editorform').down('searchcombobox[name=BrandId]').hide();
                        field.up('editorform').down('searchcombobox[name=TechnologyId]').hide();
                        field.up('editorform').down('textfield[name=Name]').show();
                        field.up('editorform').down('textfield[name=Abbreviation]').setDisabled(true);
                    }
                }
            },
            mapping: [{
                from: 'Name',
                to: 'Type'
            }]
        }, {
            xtype: 'searchcombobox',
            fieldLabel: l10n.ns('tpm', 'ProductTree').value('Name'),
            name: 'BrandId',
            selectorWidget: 'brand',
            allowBlank: true,
            allowOnlyWhitespace: true,
            valueField: 'Id',
            displayField: 'Name',
            entityType: 'Brand',
            store: {
                type: 'simplestore',
                model: 'App.model.tpm.brand.Brand',
                extendedFilter: {
                    xclass: 'App.ExtFilterContext',
                    supportedModels: [{
                        xclass: 'App.ExtSelectionFilterModel',
                        model: 'App.model.tpm.brand.Brand',
                        modelId: 'efselectionmodel'
                    }]
                }
            },
            mapping: [{
                from: 'Name',
                to: 'Name'
            }]
        }, {
            xtype: 'searchcombobox',
            fieldLabel: l10n.ns('tpm', 'ProductTree').value('Name'),
            name: 'TechnologyId',
            selectorWidget: 'technology',
            valueField: 'Id',
            displayField: 'Name',
            entityType: 'Technology',
            allowBlank: true,
            allowOnlyWhitespace: true,
            store: {
                type: 'simplestore',
                model: 'App.model.tpm.technology.Technology',
                extendedFilter: {
                    xclass: 'App.ExtFilterContext',
                    supportedModels: [{
                        xclass: 'App.ExtSelectionFilterModel',
                        model: 'App.model.tpm.technology.Technology',
                        modelId: 'efselectionmodel'
                    }]
                }
            },
            mapping: [{
                from: 'Name',
                to: 'Name'
            }]
        }, {
            xtype: 'textfield',
            name: 'Name',
            allowBlank: true,
            allowOnlyWhitespace: true,
            fieldLabel: l10n.ns('tpm', 'ProductTree').value('Name')
        }, {
            xtype: 'textfield',
            name: 'Abbreviation',
            disabled: true,
            fieldLabel: l10n.ns('tpm', 'ProductTree').value('Abbreviation')
        }]
    }]
});
