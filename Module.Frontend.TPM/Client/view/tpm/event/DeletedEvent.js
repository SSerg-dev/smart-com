Ext.define('App.view.tpm.event.DeletedEvent', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.deletedevent',
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
            model: 'App.model.tpm.event.DeletedEvent',
            storeId: 'deletedeventstore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.event.DeletedEvent',
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
                text: l10n.ns('tpm', 'Event').value('Name'),
                dataIndex: 'Name'
            }, {
                xtype: 'numbercolumn',
                format: '0',
                text: l10n.ns('tpm', 'Event').value('Year'),
                dataIndex: 'Year'
            }, {
                text: l10n.ns('tpm', 'Event').value('Period'),
                dataIndex: 'Period',
                filter: {
                    type: 'combo',
                    valueField: 'val',
                    store: {
                        type: 'simplestore',
                        id: 'val',
                        fields: ['val'],
                        data: [{ val: 'P01' }, { val: 'P02' },
                            { val: 'P03' }, { val: 'P04' }, { val: 'P05' },
                            { val: 'P06' }, { val: 'P07' }, { val: 'P08' },
                            { val: 'P09' }, { val: 'P10' }, { val: 'P11' },
                            { val: 'P12' }, { val: 'P13' }]
                    },
                    displayField: 'val',
                    queryMode: 'local',
                    operator: 'eq'
                }
            }, {
                text: l10n.ns('tpm', 'Event').value('Description'),
                dataIndex: 'Description'
            }, ]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.tpm.event.DeletedEvent',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: 'DeletedDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('core', 'BaseDeletedEntity').value('DeletedDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Name',
            fieldLabel: l10n.ns('tpm', 'Event').value('Name'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Year',
            fieldLabel: l10n.ns('tpm', 'Event').value('Year'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Period',
            fieldLabel: l10n.ns('tpm', 'Event').value('Period'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Description',
            fieldLabel: l10n.ns('tpm', 'Event').value('Description'),
        }, ]
    },]
});


