ResourceMgr.defineAdditionalMenu('tpm', {
    'ct:scheduleExport': {
        glyph: 0xf21b,
        text: l10n.ns('tpm', 'additionalMenu').value('ScheduleExport'),

        menu: {
            xtype: 'customheadermenu',
            items: [{
                glyph: 0xf21d,
                itemgroup: 'exportschedulebutton',
                text: l10n.ns('tpm', 'additionalMenu').value('ExportSchedule'),
                currentYear: false
            }, {
                glyph: 0xf21d,
                itemgroup: 'exportschedulebutton',
                text: l10n.ns('tpm', 'additionalMenu').value('ExportYearSchedule'),
                currentYear: true
            }]
        }
    },
});