Ext.define('Core.form.ColumnLayoutForm', {
    extend: 'Ext.panel.Panel',

    columnsCount: 2,

    initComponent: function () {
        this.callParent(arguments);
        this.form = this.down('form').getForm();
    },

    getForm: function () {
        return this.form;
    },

    initItems: function () {
        var me = this,
            items = me.items,
            len = items.length,
            columns = len <= 1 || me.customEdit ? 1 : Math.min(len, me.columnsCount),
            itemPerColumnCount = Math.ceil(len / columns),
            itemDefaults = Ext.apply({
                ui: 'detail-form-field',
                labelClsExtra: 'singleline-lable',
                allowBlank: false,
                allowOnlyWhitespace: false,
                labelAlign: 'left',
                labelWidth: 170,
                labelSeparator: '',
                labelPad: 0
            }, me.itemDefaults);

        if (items) {
            items = Ext.Array.from(items);

            if (columns == 1) {
                items = [{
                    items: items,
                    defaults: itemDefaults,
                    padding: '0 10 0 10'
                }]
            } else {
                var resItems = [],
                    spacer = {
                        width: 30,
                        flex: 0
                    };

                var rowIdx = 0;
                var columnItems = [];

                items.forEach(function (item, index) {
                    var isFirstColumn = index < itemPerColumnCount,
                        isLastItem = index == len - 1;

                    if (rowIdx < itemPerColumnCount) {
                        columnItems.push(item);
                    }

                    if (rowIdx >= itemPerColumnCount - 1 || isLastItem) {
                        var column = {
                            items: columnItems,
                            defaults: itemDefaults
                        };

                        if (isFirstColumn) {
                            column.padding = '0 0 0 10';
                        } else if (isLastItem) {
                            column.padding = '0 10 0 0';
                        } else {
                            column.padding = '0';
                        }

                        resItems.push(column);

                        if (!isLastItem) {
                            resItems.push(spacer);
                        }

                        columnItems = [];
                        rowIdx = 0;
                    } else {
                        rowIdx++;
                    }
                });

                items = resItems;
            }

            me.items = [{
                xtype: 'extendedpanel',
                ui: 'detailform-panel',

                layout: {
                    type: 'hbox',
                    pack: 'center'
                },

                defaults: {
                    xtype: 'container',
                    flex: 1,
                    maxWidth: columns == 1 ? 1230 : 600,

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