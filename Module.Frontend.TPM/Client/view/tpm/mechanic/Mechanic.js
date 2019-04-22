Ext.define('App.view.tpm.mechanic.Mechanic', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.mechanic',
    title: l10n.ns('tpm', 'compositePanelTitles').value('Mechanic'),

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
            model: 'App.model.tpm.mechanic.Mechanic',
            storeId: 'mechanicstore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.mechanic.Mechanic',
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
				text: l10n.ns('tpm', 'Mechanic').value('Name'),
				dataIndex: 'Name'
			}, 			{
				text: l10n.ns('tpm', 'Mechanic').value('SystemName'),
				dataIndex: 'SystemName'
			}]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.tpm.mechanic.Mechanic',
        items: [{
            xtype: 'textfield',
            name: 'Name',
            fieldLabel: l10n.ns('tpm', 'Mechanic').value('Name'),
        }, {
            xtype: 'textfield', allowBlank: true, allowOnlyWhitespace: true,
            name: 'SystemName',
            fieldLabel: l10n.ns('tpm', 'Mechanic').value('SystemName'),
        }]
    }]
});