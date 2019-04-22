Ext.define('App.view.tpm.postpromoeffect.DeletedPostPromoEffect', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.deletedpostpromoeffect',
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
            model: 'App.model.tpm.postpromoeffect.DeletedPostPromoEffect',
            storeId: 'deletedpostpromoeffectstore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.postpromoeffect.DeletedPostPromoEffect',
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
                text: l10n.ns('tpm', 'PostPromoEffect').value('StartDate'),
                dataIndex: 'StartDate',
                renderer: Ext.util.Format.dateRenderer('d.m.Y')
            }, {
                text: l10n.ns('tpm', 'PostPromoEffect').value('EndDate'),
                dataIndex: 'EndDate',
                renderer: Ext.util.Format.dateRenderer('d.m.Y')
            }, {
                text: l10n.ns('tpm', 'PostPromoEffect').value('ClientTreeFullPathName'),
                dataIndex: 'ClientTreeFullPathName',
                minWidth: 200,
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
                },
                renderer: function (value) {
                    return renderWithDelimiter(value, ' > ', '  ');
                }
            }, {
                text: l10n.ns('tpm', 'PostPromoEffect').value('ProductTreeFullPathName'),
                dataIndex: 'ProductTreeFullPathName',
                minWidth: 200,
                filter: {
                    xtype: 'treefsearchfield',
                    trigger2Cls: '',
                    selectorWidget: 'producttree',
                    valueField: 'FullPathName',
                    displayField: 'FullPathName',
                    multiSelect: true,
                    operator: 'conts',
                    store: {
                        model: 'App.model.tpm.producttree.ProductTree',
                        autoLoad: false,
                        root: {}
                    },
                },
                renderer: function (value) {
                    return renderWithDelimiter(value, ' > ', '  ');
                }
            }, {
				xtype: 'numbercolumn',
				format: '0.00',
                text: l10n.ns('tpm', 'PostPromoEffect').value('EffectWeek1'),
                dataIndex: 'EffectWeek1'
			}, {
				xtype: 'numbercolumn',
				format: '0.00',
                text: l10n.ns('tpm', 'PostPromoEffect').value('EffectWeek2'),
                dataIndex: 'EffectWeek2'
			}, {
				xtype: 'numbercolumn',
				format: '0.00',
                text: l10n.ns('tpm', 'PostPromoEffect').value('TotalEffect'),
                dataIndex: 'TotalEffect'
			}]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.tpm.postpromoeffect.DeletedPostPromoEffect',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: 'DeletedDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('core', 'BaseDeletedEntity').value('DeletedDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'StartDate',
            fieldLabel: l10n.ns('tpm', 'PostPromoEffect').value('StartDate'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'EndDate',
            fieldLabel: l10n.ns('tpm', 'PostPromoEffect').value('EndDate'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ClientTreeFullPathName',
            fieldLabel: l10n.ns('tpm', 'PostPromoEffect').value('ClientTreeFullPathName'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ProductTreeFullPathName',
            fieldLabel: l10n.ns('tpm', 'PostPromoEffect').value('ProductTreeFullPathName'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'EffectWeek1',
            fieldLabel: l10n.ns('tpm', 'PostPromoEffect').value('EffectWeek1'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'EffectWeek2',
            fieldLabel: l10n.ns('tpm', 'PostPromoEffect').value('EffectWeek2'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'TotalEffect',
            fieldLabel: l10n.ns('tpm', 'PostPromoEffect').value('TotalEffect'),
        }]
    }]
});