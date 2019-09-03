Ext.define('App.view.tpm.assortmentmatrix.AssortmentMatrix', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.assortmentmatrix',
    title: l10n.ns('tpm', 'compositePanelTitles').value('AssortmentMatrix'),

    customHeaderItems: [
        ResourceMgr.getAdditionalMenu('core').base = {
            glyph: 0xf068,
            text: l10n.ns('core', 'additionalMenu').value('additionalBtn'),

            menu: {
                xtype: 'customheadermenu',
                items: [{
                    glyph: 0xf4eb,
                    itemId: 'gridsettings',
                    text: l10n.ns('core', 'additionalMenu').value('gridSettingsMenuItem'),
                    action: 'SaveGridSettings',
                    resource: 'Security'
                }]
            }
        },
        ResourceMgr.getAdditionalMenu('core').import = {
            glyph: 0xf21b,
            text: l10n.ns('core', 'additionalMenu').value('importExportBtn'),

            menu: {
                xtype: 'customheadermenu',
                items: [{
                    glyph: 0xf220,
                    itemgroup: 'loadimportbutton',
                    exactlyModelCompare: true,
                    text: l10n.ns('core', 'additionalMenu').value('fullImportXLSX'),
                    resource: '{0}',
                    action: 'FullImportXLSX',
                    allowFormat: ['zip', 'xlsx']
                }, {
                    glyph: 0xf21d,
                    itemId: 'loadimporttemplatexlsxbutton',
                    exactlyModelCompare: true,
                    text: l10n.ns('core', 'additionalMenu').value('importTemplateXLSX'),
                    action: 'DownloadTemplateXLSX'
                }, {
                    glyph: 0xf21d,
                    itemId: 'exportxlsxbutton',
                    exactlyModelCompare: true,
                    text: l10n.ns('core', 'additionalMenu').value('exportXLSX'),
                    action: 'ExportXLSX'
                }]
            }
        }
    ],

    dockedItems: [{
        xtype: 'custombigtoolbar',
        dock: 'right',
        items: [{
            xtype: 'widthexpandbutton',
            ui: 'fill-gray-button-toolbar',
            text: l10n.ns('core', 'selectablePanelButtons').value('toolbarCollapse'),
            glyph: 0xf13d,
            glyph1: 0xf13e,
            target: function () {
                return this.up('toolbar');
            },
        }, {
            itemId: 'createbutton',
            action: 'Post',
            glyph: 0xf415,
            text: l10n.ns('core', 'crud').value('createButtonText'),
            tooltip: l10n.ns('core', 'crud').value('createButtonText')
        }, {
            itemId: 'updatebutton',
            action: 'Patch',
            glyph: 0xf64f,
            text: l10n.ns('core', 'crud').value('updateButtonText'),
            tooltip: l10n.ns('core', 'crud').value('updateButtonText')
        }, {
            itemId: 'deletebutton',
            action: 'Delete',
            glyph: 0xf5e8,
            text: l10n.ns('core', 'crud').value('deleteButtonText'),
            tooltip: l10n.ns('core', 'crud').value('deleteButtonText')
        }, {
            itemId: 'historybutton',
            resource: 'Historical{0}',
            action: 'Get{0}',
            glyph: 0xf2da,
            text: l10n.ns('core', 'crud').value('historyButtonText'),
            tooltip: l10n.ns('core', 'crud').value('historyButtonText')
        }, '-', {
            itemId: 'extfilterbutton',
            glyph: 0xf349,
            text: l10n.ns('core', 'toptoolbar').value('filterButtonText'),
            tooltip: l10n.ns('core', 'toptoolbar').value('filterButtonText')
        }, {
            itemId: 'deletedbutton',
            resource: 'Deleted{0}',
            action: 'Get{0}',
            glyph: 0xf258,
            text: l10n.ns('core', 'toptoolbar').value('deletedButtonText'),
            tooltip: l10n.ns('core', 'toptoolbar').value('deletedButtonText')
        }, {
            itemId: 'actualassortmentmatrix',
            action: 'Post',
            glyph: 0xf3cc,
            text: l10n.ns('tpm', 'AssortmentMatrix').value('actualAssortmentMatrixButtonText'),
            tooltip: l10n.ns('tpm', 'AssortmentMatrix').value('actualAssortmentMatrixButtonText')
        }, '-', '->', '-', {
            itemId: 'extfilterclearbutton',
            ui: 'blue-button-toolbar',
            disabled: true,
            glyph: 0xf232,
            text: l10n.ns('core', 'filter').value('filterEmptyStatus'),
            tooltip: l10n.ns('core', 'filter').value('filterEmptyStatus'),
            overCls: '',
            style: {
                'cursor': 'default'
            }
        }]
    }],

    items: [{
        xtype: 'directorygrid',
        itemId: 'datatable',
        editorModel: 'Core.form.EditorDetailWindowModel',
        store: {
            type: 'directorystore',
            model: 'App.model.tpm.assortmentmatrix.AssortmentMatrix',
            storeId: 'assortmentmatrixstore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.assortmentmatrix.AssortmentMatrix',
                    modelId: 'efselectionmodel'
                }, {
                    xclass: 'App.ExtTextFilterModel',
                    modelId: 'eftextmodel'
                }]
            },
            sorters: [{
                property: 'Number',
                direction: 'DESC'
            }]
        },

        columns: {
            defaults: {
                plugins: ['sortbutton'],
                menuDisabled: true,
                filter: true,
                flex: 1,
                minWidth: 100
            },
            items: [{
                text: l10n.ns('tpm', 'AssortmentMatrix').value('Number'),
                dataIndex: 'Number',
            }, {
                text: l10n.ns('tpm', 'AssortmentMatrix').value('ClientTreeName'),
                dataIndex: 'ClientTreeName',
                width: 250,
                filter: {
                    xtype: 'treefsearchfield',
                    trigger2Cls: '',
                    selectorWidget: 'clienttree',
                    valueField: 'Name',
                    displayField: 'Name',
                    multiSelect: true,
                    operator: 'conts',
                    store: {
                        model: 'App.model.tpm.clienttree.ClientTree',
                        autoLoad: false,
                        root: {}
                    },
                }
            }, {
                text: l10n.ns('tpm', 'NoneNego').value('ClientTreeObjectId'),
                dataIndex: 'ClientTreeObjectId'
            }, {
                text: l10n.ns('tpm', 'AssortmentMatrix').value('EAN_PC'),
                dataIndex: 'ProductEAN_PC',
                filter: {
                    type: 'search',
                    selectorWidget: 'product',
                    valueField: 'EAN_PC',
                    store: {
                        type: 'directorystore',
                        model: 'App.model.tpm.product.Product',
                        extendedFilter: {
                            xclass: 'App.ExtFilterContext',
                            supportedModels: [{
                                xclass: 'App.ExtSelectionFilterModel',
                                model: 'App.model.tpm.product.Product',
                                modelId: 'efselectionmodel'
                            }, {
                                xclass: 'App.ExtTextFilterModel',
                                modelId: 'eftextmodel'
                            }]
                        }
                    }
                }
            }, {
                text: l10n.ns('tpm', 'AssortmentMatrix').value('StartDate'),
                dataIndex: 'StartDate',
                renderer: Ext.util.Format.dateRenderer('d.m.Y')
            }, {
                text: l10n.ns('tpm', 'AssortmentMatrix').value('EndDate'),
                dataIndex: 'EndDate',
                renderer: Ext.util.Format.dateRenderer('d.m.Y')
            }, {
                text: l10n.ns('tpm', 'AssortmentMatrix').value('CreateDate'),
                dataIndex: 'CreateDate',
                renderer: Ext.util.Format.dateRenderer('d.m.Y')
            }]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.tpm.assortmentmatrix.AssortmentMatrix',
        items: [{
            xtype: 'treesearchfield',
            name: 'ClientTreeId',
            text: l10n.ns('tpm', 'AssortmentMatrix').value('ClientTreeName'),
            selectorWidget: 'clienttree',
            valueField: 'Id',
            displayField: 'Name',
            store: {
                storeId: 'clienttreestore',
                model: 'App.model.tpm.clienttree.ClientTree',
                autoLoad: false,
                root: {}
            },
            mapping: [{
                from: 'Name',
                to: 'ClientTreeName'
            }]
        }, {
            xtype: 'editabledetailform',
            itemId: 'detailform',
            model: 'App.model.tpm.assortmentmatrix.AssortmentMatrix',
            items: [{
                xtype: 'searchfield',
                name: 'ProductId',
                fieldLabel: l10n.ns('tpm', 'AssortmentMatrix').value('EAN_PC'),
                selectorWidget: 'product',
                valueField: 'Id',
                displayField: 'EAN_PC',
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
                    from: 'EAN_PC',
                    to: 'EAN_PC'
                }]
            }]
        }]
    }]
});