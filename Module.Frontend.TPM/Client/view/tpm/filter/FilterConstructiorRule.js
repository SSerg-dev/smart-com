Ext.define('App.view.tpm.filter.FilterConstructiorRule', {
    extend: 'Ext.container.Container',
    alias: 'widget.filterconstructorrule',
    margin: 1,
    cls: 'filterrulecontainer',

    layout: 'anchor',
    defaults: {
        anchor: '100%'
    },

    items: [{
        xtype: 'container',
        layout: {
            type: 'hbox',
            align: 'stretch'
        },
        items: [{
            xtype: 'combobox',
            name: 'filterfield',
            cls: 'filterCombobox',
            disabledCls: 'disabledFilterCombobox',
            flex: 4,
            margin: 1,
            queryMode: 'local',
            allowBlank: false,
            valueField: 'Field',
            displayField: 'DisplayName',
            emptyText: 'Field'
        }, {
            xtype: 'combobox',
            name: 'operationfield',
            cls: 'filterCombobox',
            disabledCls: 'disabledFilterCombobox',
            editable: false,
            flex: 3,
            margin: 1,
            valueField: 'id',
            queryMode: 'local',
            store: {
                fields: ['id', 'text']
            }
        }, {
            xtype: 'container',
            name: 'valuecontainer',
            flex: 3,
            layout: 'fit'
        }, {
            xtype: 'button',
            itemId: 'delete',
            glyph: 0xf156,
            cls: 'transparentbutton',
            margin: 1
        }]
    }]
});