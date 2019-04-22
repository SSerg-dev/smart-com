Ext.define('App.view.tpm.baseclienttreeview.BaseClientTreeView', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.baseclienttreeview',
    title: l10n.ns('tpm', 'compositePanelTitles').value('BaseClientTreeView'),

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
            model: 'App.model.tpm.baseclienttreeview.BaseClientTreeView',
            storeId: 'baseclienttreeviewstore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.baseclienttreeview.BaseClientTreeView',
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
                text: l10n.ns('tpm', 'BaseClientTreeView').value('ResultNameStr'),
                dataIndex: 'ResultNameStr',
                filter: {
                    type: 'string',
                    operator: 'conts',
                    onChange: function (value) {
                        if (value) {
                            this.setValue(value.replace(//g, '>'));
                        }
                    },
                },
                renderer: function (value) {
                    return renderWithDelimiter(value, ' > ', '  ');
                }
            }, {
                text: l10n.ns('tpm', 'BaseClientTreeView').value('BOI'),
                dataIndex: 'BOI'
            }]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.tpm.baseclienttreeview.BaseClientTreeView',
        items: [{
            xtype: 'textfield',
            name: 'ResultNameStr',
            fieldLabel: l10n.ns('tpm', 'BaseClientTreeView').value('ResultNameStr'),
            renderer: function (value) {
                return renderWithDelimiter(value, ' > ', '  ');
            }
        }, {
            xtype: 'textfield',
            name: 'BOI',
            fieldLabel: l10n.ns('tpm', 'BaseClientTreeView').value('BOI')
        }]
    }]
});
