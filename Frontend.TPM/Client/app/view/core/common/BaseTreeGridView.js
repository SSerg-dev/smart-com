Ext.define('App.view.core.common.BaseTreeGridView', {
    extend: 'Ext.tree.Panel',
    alias: 'widget.basetreegridview',

    useArrows: true,
    rootVisible: false,

    cls: 'directorygrid',
    bodyCls: 'border-body',
    baseCls: 'base-client-tree',

    enableColumnHide: false,
    enableColumnMove: false,
    columnLines: true,
    rowLines: true,

    animCollapse: false,
    animate: false,

    scroll: true,

    //dockedItems: [{
    //    xtype: 'gridinfotoolbar',
    //    dock: 'bottom',
    //    cls: 'grid-info-toolbar',
    //    height: 10
    //}],

    viewConfig: {
        overflowX: 'scroll',
        overflowY: 'scroll',
        enableTextSelection: true,
        trackOver: false,
        animate: true,
        preserveScrollOnRefresh: true,
        stripeRows: false,
        multiSelect: true,
        rootVisible: false,
        loadMask: {
            maskOnDisable: false
        },
        listeners: {
            checkchange: function (node, checked) {
                node.cascadeBy(function (n) { n.set('checked', checked); });
            },
            addhorizontalscroll: function (view) {
                // Изменение высоты футера из-за появления горизонтальной прокрутки
                //view.panel.down('gridinfotoolbar').addCls('grid-info-toolbar-has-scroll');
            }
        }
    },

    //store: Ext.create('Ext.data.TreeStore', {
    //    root: {
    //        text: "Clients",
    //        expanded: true,
    //        checked: false,
    //        children: [
    //            { text: "Menu Option 1", leaf: true, checked: false },
    //            {
    //                text: "Menu Option 2", expanded: true, checked: false,
    //                children: [
    //                    { text: "Sub Menu Option 2.1", leaf: true, checked: false },
    //                    { text: "Sub Menu Option 2.2", leaf: true, checked: false }
    //                ]
    //            },
    //            { text: "Menu Option 3", leaf: true, checked: false }
    //        ]
    //    }
    //}),
});
