Ext.define('App.view.core.constraint.Constraint', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.constraint',
    title: l10n.ns('core', 'compositePanelTitles').value('ConstraintTitle'),

    dockedItems: [{
        xtype: 'harddeletedirectorytoolbar',
        dock: 'right'
    }],

    //items: [{
    //    xtype: 'directorygrid',
    //    itemId: 'datatable',
    //    editorModel: 'Core.form.EditorWindowModel',
    //    store: {
    //        type: 'associateddirectorystore',
    //        model: 'App.model.core.constraint.Constraint',
    //        storeId: 'constraintstore',
    //        extendedFilter: {
    //            xclass: 'App.ExtFilterContext',
    //            supportedModels: [{
    //                xclass: 'App.ExtSelectionFilterModel',
    //                model: 'App.model.core.constraint.Constraint',
    //                modelId: 'efselectionmodel'
    //            }, {
    //                xclass: 'App.ExtTextFilterModel',
    //                modelId: 'eftextmodel'
    //            }]
    //        }
    //    },

    //    columns: {
    //        defaults: {
    //            plugins: ['sortbutton'],
    //            menuDisabled: true,
    //            filter: true,
    //            flex: 1,
    //            minWidth: 100
    //        },
    //        items: [{ 
    //			text: l10n.ns('core', 'Constraint').value('Prefix'),
    //			dataIndex: 'Prefix',
    //			renderer: App.RenderHelper.getLocalizedRenderer('core', 'Constraint', 'ConstraintPrefixEnum')
    //		}, { 
    //			text: l10n.ns('core', 'Constraint').value('Value'),
    //			dataIndex: 'Value'
    //		}]
    //    }
    //}, {
    //    xtype: 'editabledetailform',
    //    itemId: 'detailform',
    //    model: 'App.model.core.constraint.Constraint',
    //    items: [{
    //        xtype: 'textfield',
    //		name: 'Prefix',
    //		fieldLabel: l10n.ns('core', 'Constraint').value('Prefix'),
    //		renderer: App.RenderHelper.getLocalizedRenderer('core', 'Constraint', 'ConstraintPrefixEnum')
    //    }, {
    //        xtype: 'textfield',
    //		name: 'Value',
    //		fieldLabel: l10n.ns('core', 'Constraint').value('Value')
    //	}]
    //}]
});