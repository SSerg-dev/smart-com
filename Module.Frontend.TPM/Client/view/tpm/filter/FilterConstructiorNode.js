Ext.define('App.view.tpm.filter.FilterConstructiorNode', {
    extend: 'Ext.container.Container',
    alias: 'widget.filterconstructornode',
    margin: 5,

    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    items: [{
        xtype: 'container',
        layout: {
            type: 'hbox',
            align: 'stretch'
        },
        items: [{
            xtype: 'button',
            itemId: 'operationButton',
            cls: 'operationbutton'
        }, {
            xtype: 'tbspacer',
            flex: 1
        }, {
            xtype: 'button',
            text: 'Add panel',
            itemId: 'addNode',
            tooltip: 'Add panel',
            glyph: 0xf328,
            cls: 'transparentbutton',
            height: 20,
            margin: 1
        }, {
            xtype: 'button',
            itemId: 'delete',
            text: 'Delete',
            glyph: 0xf156,
            disabled: true,
            cls: 'transparentbutton',
            height: 20,
            margin: 1
        }]
    }, {
        xtype: 'container',
        name: 'rulecontent',
    }, {
        xtype: 'container',
        layout: {
            type: 'hbox',
            align: 'stretch'
        },
        margin: '0 0 0 60',
        items:[{
            xtype: 'button',
            itemId: 'addRule',
            text: 'Add search rule',
            glyph: 0xf415,
            cls: 'transparentbutton'
        }, {
            xtype: 'tbspacer',
            flex: 1
        }]
    }, {
        xtype: 'container',
        name: 'content'
    }]
});