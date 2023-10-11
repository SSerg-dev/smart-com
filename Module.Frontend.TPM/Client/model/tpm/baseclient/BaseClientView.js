Ext.define('App.model.tpm.baseclient.BaseClientView', {
    extend: 'Ext.data.Model',
    idProperty: 'objectId',
    //breezeEntityType: 'ClientTree',
    fields: [
        { name: 'id', type: 'int', type: "int" },
        { name: 'objectId', type: "int" },
        { name: "text", type: "string" },
        { name: "leaf", type: "bool" },
        { name: "expanded", type: "bool" },
        { name: "checked", type: "bool" }
    ],

    proxy: {
        type: 'ajax',
        //resourceName: 'BaseClientViews',
        api: {
            read: 'api/BaseClientViews'
        },
        reader: {
            type: 'json',
            root: 'children',
            successProperty: 'success',
            //getData: function (data) {
            //    debugger;
            //    var value = Ext.JSON.decode(data);
            //    //var clientbaseviewstore = Ext.data.StoreManager.lookup('clientbaseviewstore');
            //    //clientbaseviewstore.setRootNode(value);
            //    console.log(value);
            //    return value;
            //}
        }
    }
});