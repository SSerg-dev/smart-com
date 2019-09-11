ResourceMgr.defineAdditionalMenu('core', {
    'ct:base': {
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
    'ct:loophandler': {
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
            }, {
                glyph: 0xf219,
                itemId: 'taskdetails',
                resource: 'LoopHandlers',
                action: 'Parameters',
                text: l10n.ns('core', 'additionalMenu').value('taskDetailsMenuItem')
            }, {
                glyph: 0xf459,
                itemId: 'loophandlerrestart',
                resource: 'LoopHandlers',
                action: 'Restart',
                text: l10n.ns('core', 'additionalMenu').value('restartMenuItem')
            }]
        }
    },
    'ct:import': {
        glyph: 0xf21b,
        text: l10n.ns('core', 'additionalMenu').value('importExportBtn'),

        menu: {
            xtype: 'customheadermenu',
            items: [{
                glyph: 0xf216,
                itemId: 'applyimportbutton',
                exactlyModelCompare: true,
                text: l10n.ns('core', 'additionalMenu').value('applyImport'),
                action: 'Apply',
                resource: '{0}'
            }, {
                glyph: 0xf220,
                itemgroup: 'loadimportbutton',
                exactlyModelCompare: true,
                text: l10n.ns('core', 'additionalMenu').value('importCSV'),
                resource: 'Import{0}',
                action: 'ImportCSV'
            }, {
                glyph: 0xf220,
                itemgroup: 'loadimportbutton',
                exactlyModelCompare: true,
                text: l10n.ns('core', 'additionalMenu').value('importXLSX'),
                resource: 'Import{0}',
                action: 'ImportXLSX'
            }, {
                glyph: 0xf220,
                itemgroup: 'loadimportbutton',
                exactlyModelCompare: true,
                text: l10n.ns('core', 'additionalMenu').value('fullImportCSV'),
                resource: 'Import{0}',
                action: 'FullImportCSV'
            }, {
                glyph: 0xf220,
                itemgroup: 'loadimportbutton',
                exactlyModelCompare: true,
                text: l10n.ns('core', 'additionalMenu').value('fullImportXLSX'),
                resource: 'Import{0}',
                action: 'FullImportXLSX'
            }, {
                glyph: 0xf224,
                itemId: 'loadimporttemplatecsvbutton',
                exactlyModelCompare: true,
                text: l10n.ns('core', 'additionalMenu').value('importTemplateCSV'),
                action: 'DownloadTemplateCSV'
            }, {
                glyph: 0xf224,
                itemId: 'loadimporttemplatexlsxbutton',
                exactlyModelCompare: true,
                text: l10n.ns('core', 'additionalMenu').value('importTemplateXLSX'),
                action: 'DownloadTemplateXLSX'
            }, {
                glyph: 0xf21d,
                itemId: 'exportcsvbutton',
                exactlyModelCompare: true,
                text: l10n.ns('core', 'additionalMenu').value('exportCSV'),
                action: 'ExportCSV'
            }, {
                glyph: 0xf21d,
                itemId: 'exportxlsxbutton',
                exactlyModelCompare: true,
                text: l10n.ns('core', 'additionalMenu').value('exportXLSX'),
                action: 'ExportXLSX'
            }]
        }
    },
    'ct:filebuffer': {
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
            }, {
                glyph: 0xf40d,
                itemId: 'manualprocess',
                resource: 'FileBuffers',
                action: 'ManualProcess',
                text: l10n.ns('core', 'additionalMenu').value('manualProcess')
            }, {
                glyph: 0xf48a,
                itemId: 'manualsend',
                resource: 'FileBuffers',
                action: 'ManualSend',
                text: l10n.ns('core', 'additionalMenu').value('manualSend')
            }, {
                glyph: 0xf192,
                itemId: 'downloadbufferfile',
                resource: 'File',
                action: 'DownloadBufferFile',
                text: l10n.ns('core', 'additionalMenu').value('manualDownload')
            }, {
                glyph: 0xf478,
                itemId: 'readfilebufferlog',
                resource: 'FileBuffers',
                action: 'ReadLogFile',
                text: l10n.ns('core', 'additionalMenu').value('readLog')
            }, {
                glyph: 0xf2fd,
                itemId: 'importresultfiles',
                resource: 'File',
                action: 'ImportResultSuccessDownload',
                text: l10n.ns('core', 'additionalMenu').value('importResults')
            }]
        }
    }
});