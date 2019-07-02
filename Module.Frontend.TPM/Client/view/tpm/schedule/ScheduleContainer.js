Ext.define('App.view.tpm.schedule.ScheduleContainer', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.schedulecontainer',

    dockedItems: [{
        xtype: 'custombigtoolbar',
        dock: 'right',
        items: [{
            xtype: 'widthexpandbutton',
            ui: 'fill-gray-button-toolbar',
            text: l10n.ns('core', 'selectablePanelButtons').value('toolbarCollapse'),
            glyph: 0xf13d,
            glyph1: 0xf13e,
            target: function () {
                return this.up('toolbar');
            },
            toggleCollapse: function () {
                var target = this.getTarget();
                var isCollapsed = this.isCollapsed();
                target.setWidth(isCollapsed ? target.maxWidth : target.minWidth);
                if (isCollapsed) {
                    target.down('#createbutton').setUI('create-promo-btn-toolbar-expanded');
                    target.down('#createbutton').setText(l10n.ns('tpm', 'Schedule').value('CreateExpanded'));
                   // target.down('#createinoutbutton').setUI('create-promo-btn-toolbar-expanded');
                   // target.down('#createinoutbutton').setText(l10n.ns('tpm', 'Schedule').value('CreateInOutExpanded'));

                } else {
                    target.down('#createbutton').setUI('create-promo-btn-toolbar');
                    target.down('#createbutton').setText(l10n.ns('tpm', 'Schedule').value('CreateCollapsed'));
                   // target.down('#createinoutbutton').setUI('create-promo-btn-toolbar');
                   // target.down('#createinoutbutton').setText(l10n.ns('tpm', 'Schedule').value('CreateInOutCollapsed'));
                }
                target.isExpanded = !target.isExpanded;
            },
        }, {
            itemId: 'createbutton',
            action: 'Post',
            glyph: 0xf0f3,
            text: l10n.ns('tpm', 'Schedule').value('CreateCollapsed'),
            tooltip: l10n.ns('tpm', 'Schedule').value('CreateCollapsed'),
            ui: 'create-promo-btn'
            },
        //кнопка 'Create Promo InOut' временно скрыта
            //{
            //    itemId: 'createinoutbutton',
            //    action: 'Post',
            //    glyph: 0xf0f3,
            //    text: l10n.ns('tpm', 'Schedule').value('CreateInOutCollapsed'),
            //    tooltip: l10n.ns('tpm', 'Schedule').value('CreateInOutCollapsed'),
            //    ui: 'create-promo-btn'
            //},
        //{
        //    xtype: 'schedulemultiselectbutton',
        //    text: l10n.ns('tpm', 'Schedule').value('Multiselectbutton'),
        //    tooltip: l10n.ns('tpm', 'Schedule').value('Multiselectbutton'),
        //    iconCls: 'scheduler-custom-btn'
        //icon: 'Bundles/style/images/calendar-multiselect.png',
        //ui: 'gray-button-toolbar',
        //padding: 6, //TODO: временно
        //textAlign: 'left',
        //cls:"x-btn-icon-el  x-btn-glyph"
        //},
        {
            itemId: 'schedulefilterdraftpublbutton',
            text: l10n.ns('tpm', 'Schedule').value('DraftPublFilter'),
            tooltip: l10n.ns('tpm', 'Schedule').value('DraftPublFilter'),
            glyph: 0xf0f5,
        }, {
            itemId: 'schedulefilterdraftbutton',
            text: l10n.ns('tpm', 'Schedule').value('DraftFilter'),
            tooltip: l10n.ns('tpm', 'Schedule').value('DraftFilter'),
            glyph: 0xf0ef,
        }, {
            itemId: 'extfilterbutton',
            glyph: 0xf349,
            text: l10n.ns('core', 'toptoolbar').value('filterButtonText'),
            tooltip: l10n.ns('core', 'toptoolbar').value('filterButtonText')
        }, {
            itemId: 'deletedbutton',
            resource: 'DeletedPromoes',
            action: 'GetDeletedPromoes',
            glyph: 0xf258,
            text: l10n.ns('core', 'toptoolbar').value('deletedButtonText'),
            tooltip: l10n.ns('core', 'toptoolbar').value('deletedButtonText')
        }, {
            xtype: 'schedulecheckbutton',
            text: l10n.ns('tpm', 'Schedule').value('Check'),
            tooltip: l10n.ns('tpm', 'Schedule').value('Check'),
            glyph: 0xf138,
        }, '->', '-', {
            itemId: 'extfilterclearbutton',
            ui: 'blue-button-toolbar',
            disabled: true,
            glyph: 0xf232,
            text: l10n.ns('core', 'filter').value('filterEmptyStatus'),
            tooltip: l10n.ns('core', 'filter').value('filterEmptyStatus'),
            overCls: '',
            style: {
                'cursor': 'default'
            }
        }
        ]
    }],
    customHeaderItems: [{
        itemgroup: 'shiftmodebutton',
        text: Ext.String.format('Calendar type  {0}  {1}  {2}', '<span class="mdi mdi-arrow-left-drop-circle scheduler-modebutton-text"></span><span style="font-size: 15px;">', l10n.ns('tpm', 'Schedule').value('NAMARS'), '</span><span class="mdi mdi-arrow-right-drop-circle scheduler-modebutton-text"></span>'),
        marsMode: false,
        cls: 'scheduler-shift-mode-btn',
        setMarsMode: function (value) {
            this.marsMode = value;
        }
    }, {
        xtype: 'tbspacer',
        flex: 200,
        cls: ''
    }, {
        xtype: 'button',
        text: l10n.ns('tpm', 'Schedule').value('ExportYearSchedule'),
        glyph: 0xf21d,
        itemId: 'ExportYearSchedule'
    }, {
        itemgroup: 'shiftpresetbutton',
        text: l10n.ns('tpm', 'Schedule').value('Month'),
        presetId: 'marsdayWeek',
        active: false
    }, {
        itemgroup: 'shiftpresetbutton',
        text: l10n.ns('tpm', 'Schedule').value('Period'),
        presetId: 'marsweekMonth',
        active: true
    }, {
        itemgroup: 'shiftpresetbutton',
        text: l10n.ns('tpm', 'Schedule').value('Year'),
        presetId: 'marsmonthQuarter',
        active: false
    }, {
        glyph: 0xf053,
        itemId: 'shiftprevbutton',
    }, {
        glyph: 0xf05a,
        itemId: 'shiftnextbutton',
    }],
    items: [{
        xtype: 'scheduler',
        region: 'center'
    }]
});