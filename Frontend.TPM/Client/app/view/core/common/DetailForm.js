Ext.define('App.view.core.common.DetailForm', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.detailform',
    ui: 'detailform-panel',
    cls: 'detailpanel',
    overflowY: 'auto',

    dockedItems: [{
        xtype: 'toolbar',
        dock: 'bottom',
        ui: 'footer',

        layout: {
            type: 'hbox',
            pack: 'center'
        },

        defaults: {
            ui: 'white-button-footer-toolbar',
            width: 135
        },

        items: [{
            itemId: 'prev',
            text: l10n.ns('core', 'selectablePanelButtons').value('prev'),
            glyph: 0xf04d,
            margin: '5 10 5 0'
        }, {
            itemId: 'next',
            text: l10n.ns('core', 'selectablePanelButtons').value('next'),
            glyph: 0xf054,
            iconAlign: 'right',
            margin: '5 0 5 0'
        }]
    }],

    initItems: function () {
        var me = this,
            items = me.items,
            len = items.length,
            columns = len <= 1 ? 1 : 2,
            itemPerColumnCount = Math.ceil(len / columns),
            firstColumnItems = [],
            secondColumnItems = [],
            isFirstColumn,
            itemDefaults = {
                xtype: 'singlelinedisplayfield',
                ui: 'detail-form-field',
                labelAlign: 'left',
                labelWidth: 170,
                labelSeparator: '',
                labelPad: 0
            };

        if (items) {
            items = Ext.Array.from(items);

            if (columns == 1) {
                items = [{
                    items: items,
                    defaults: itemDefaults,
                    padding: '0 10 0 10'
                }];
            } else {
                items.forEach(function (item, index) {
                    isFirstColumn = index <= itemPerColumnCount - 1;

                    if (isFirstColumn) {
                        firstColumnItems.push(item);
                    } else {
                        secondColumnItems.push(item);
                    }
                });

                items = [{
                    items: firstColumnItems,
                    defaults: itemDefaults,
                    padding: '0 0 0 10'
                }, {
                    width: 30,
                    flex: 0
                }, {
                    items: secondColumnItems,
                    defaults: itemDefaults,
                    padding: '0 10 0 0'
                }];
            }

            me.items = [{
                xtype: 'form',
                ui: 'detailform-panel',
                margin: '35 0 0 0',

                layout: {
                    type: 'hbox',
                    pack: 'center'
                },

                defaults: {
                    xtype: 'container',
                    flex: 1,
                    maxWidth: 600,

                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    }
                },

                items: items
            }];
        }

        this.callParent();
    }
});