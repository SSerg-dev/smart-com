Ext.define('App.view.tpm.durationrange.DurationRange', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.durationrange',
    title: l10n.ns('tpm', 'compositePanelTitles').value('DurationRange'),

    dockedItems: [{
        xtype: 'custombigtoolbar',
        dock: 'right'
    }],
    items: [{
        xtype: 'directorygrid',
        itemId: 'datatable',
        editorModel: 'Core.form.EditorDetailWindowModel',
        store: {
            type: 'directorystore',
            model: 'App.model.tpm.durationrange.DurationRange',
            storeId: 'durationrangestore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.durationrange.DurationRange',
                    modelId: 'efselectionmodel'
                }, {
                    xclass: 'App.ExtTextFilterModel',
                    modelId: 'eftextmodel'
                }]
            }
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
                text: l10n.ns('tpm', 'DurationRange').value('Name'),
                dataIndex: 'Name'
            }, {
                text: l10n.ns('tpm', 'DurationRange').value('MinValue'),
                dataIndex: 'MinValue'
            }, {
                text: l10n.ns('tpm', 'DurationRange').value('MaxValue'),
                dataIndex: 'MaxValue'
            }]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.tpm.durationrange.DurationRange',
        items: [{
            xtype: 'textfield',
            fieldLabel: l10n.ns('tpm', 'DurationRange').value('Name'),
            name: 'Name',
        }, {
            xtype: 'numberfield',
            fieldLabel: l10n.ns('tpm', 'DurationRange').value('MinValue'),
            name: 'MinValue',
        }, {
            xtype: 'numberfield',
            fieldLabel: l10n.ns('tpm', 'DurationRange').value('MaxValue'),
            name: 'MaxValue',
        }]
    }]
});
