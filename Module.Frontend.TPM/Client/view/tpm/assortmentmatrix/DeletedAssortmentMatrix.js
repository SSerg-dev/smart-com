Ext.define('App.view.tpm.assortmentmatrix.DeletedAssortmentMatrix', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.deletedassortmentmatrix',
    title: l10n.ns('core', 'compositePanelTitles').value('deletedPanelTitle'),

    dockedItems: [{
        xtype: 'readonlydeleteddirectorytoolbar',
        dock: 'right'
    }],

    items: [{
        xtype: 'directorygrid',
        itemId: 'datatable',
		editorModel: 'Core.form.EditorDetailWindowModel',
        store: {
            type: 'directorystore',
            model: 'App.model.tpm.assortmentmatrix.DeletedAssortmentMatrix',
            storeId: 'deletedassortmentmatrix',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.assortmentmatrix.DeletedAssortmentMatrix',
                    modelId: 'efselectionmodel'
                }, {
                    xclass: 'App.ExtTextFilterModel',
                    modelId: 'eftextmodel'
                }]
            },
            sorters: [{
                property: 'DeletedDate',
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
                text: l10n.ns('core', 'BaseDeletedEntity').value('DeletedDate'),
				dataIndex: 'DeletedDate',
				xtype: 'datecolumn',
				renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s')
			}, {
                text: l10n.ns('tpm', 'AssortmentMatrix').value('ClientTreeName'),
                dataIndex: 'ClientTreeName',
                width: 250,
                filter: {
                    xtype: 'treefsearchfield',
                    trigger2Cls: '',
                    selectorWidget: 'clienttree',
                    valueField: 'FullPathName',
                    displayField: 'FullPathName',
                    multiSelect: true,
                    operator: 'conts',
                    store: {
                        model: 'App.model.tpm.clienttree.ClientTree',
                        autoLoad: false,
                        root: {}
                    },
                }
            }, {
                text: l10n.ns('tpm', 'AssortmentMatrix').value('EAN_PC'),
                dataIndex: 'EAN_PC',
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
            },{
                text: l10n.ns('tpm', 'AssortmentMatrix').value('StartDate'),
                dataIndex: 'StartDate',
                renderer: Ext.util.Format.dateRenderer('d.m.Y')
            },{
                text: l10n.ns('tpm', 'AssortmentMatrix').value('EndDate'),
                dataIndex: 'EndDate',
                renderer: Ext.util.Format.dateRenderer('d.m.Y')
            },{
                text: l10n.ns('tpm', 'AssortmentMatrix').value('CreateDate'),
                dataIndex: 'CreateDate',
                renderer: Ext.util.Format.dateRenderer('d.m.Y')
            }]
        }
    }, {
            xtype: 'editabledetailform',
            itemId: 'detailform',
            model: 'App.model.tpm.assortmentmatrix.DeletedAssortmentMatrix',
            items: [{
                xtype: 'singlelinedisplayfield',
                name: 'DeletedDate',
                renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
                fieldLabel: l10n.ns('core', 'BaseDeletedEntity').value('DeletedDate')
            }, {
                xtype: 'singlelinedisplayfield',
                name: 'ClientTreeId',
                fieldLabel: l10n.ns('tpm', 'AssortmentMatrix').value('ClientTreeName'),
            },{
                xtype: 'singlelinedisplayfield',
                name: 'EAN_PC',
                fieldLabel: l10n.ns('tpm', 'AssortmentMatrix').value('EAN_PC')
            },{
                xtype: 'singlelinedisplayfield',
                name: 'StartDate',
                fieldLabel: l10n.ns('tpm', 'AssortmentMatrix').value('StartDate'),
            },{
                xtype: 'singlelinedisplayfield',
                name: 'EndDate',
                fieldLabel: l10n.ns('tpm', 'AssortmentMatrix').value('EndDate'),
            },{
                xtype: 'singlelinedisplayfield',
                name: 'CreateDate',
                fieldLabel: l10n.ns('tpm', 'AssortmentMatrix').value('CreateDate'),
            }]
        }]
});
