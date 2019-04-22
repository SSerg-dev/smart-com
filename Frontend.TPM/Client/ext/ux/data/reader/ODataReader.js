Ext.define('Ext.ux.data.reader.ODataReader', {
    extend: 'Ext.data.reader.Json',
    alias: 'reader.odata',
    type: 'json',
    totalProperty: '@odata.count',
    metaProperty: '@odata.context',
    useSimpleAccessors: true

    //readRecords: function () {
    //    //if (data.status == 204) {
    //    //    return null;
    //    //}

    //    //return this.self.prototype.readRecords.call(this, data['value']);
    //    return this.callParent(arguments);
    //},

    //buildExtractors: function () {
    //    //this.self.prototype.buildExtractors.apply(this, arguments);
    //    this.callParent(arguments);
    //    this.getRoot = this.getRootFunc;
    //},

    //getRootFunc: function (root) {
    //    return root['results'] || [root];
    //}

});