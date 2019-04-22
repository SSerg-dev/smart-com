Ext.define('App.view.tpm.budget.Budget', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.budget',
    title: l10n.ns('tpm', 'compositePanelTitles').value('Budget'),

    dockedItems: [{
        xtype: 'custombigtoolbar',
        dock: 'right'
    }],
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
    items: [{
        xtype: 'directorygrid',
        itemId: 'datatable',
        editorModel: 'Core.form.EditorDetailWindowModel',
        store: {
            type: 'directorystore',
            model: 'App.model.tpm.budget.Budget',
            storeId: 'budgetstore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.budget.Budget',
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
                text: l10n.ns('tpm', 'Budget').value('Name'),
                dataIndex: 'Name'
            }]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.tpm.budget.Budget',
        items: [{
            xtype: 'textfield',
            name: 'Name',
            fieldLabel: l10n.ns('tpm', 'Budget').value('Name')
        }]
    }]
});
