Ext.define('App.model.tpm.baseclient.BaseClient', {
    extend: 'Sch.model.Resource',
    mixins: ['Ext.data.Model'],
    idProperty: 'ObjectId',
    fields: [
        { name: 'Id', type: 'int', hidden: true },
        { name: 'ObjectId', hidden: true },
        { name: 'Type', type: 'string', hidden: false, isDefault: true },
        { name: 'Name', type: 'string', hidden: false, isDefault: true },
        { name: 'ExecutionCode', type: 'string', hidden: false, isDefault: true },
        { name: 'DemandCode', type: 'string', hidden: false, isDefault: true },
        { name: 'Share', type: 'int' },

        { name: 'IsBeforeStart', type: 'bool', hidden: false, useNull: true },
        { name: 'IsDaysStart', type: 'bool', hidden: false, useNull: true },
        { name: 'DaysStart', type: 'int', hidden: false, useNull: true },

        { name: 'IsBeforeEnd', type: 'bool', hidden: false, useNull: true },
        { name: 'IsDaysEnd', type: 'bool', hidden: false, useNull: true },
        { name: 'DaysEnd', type: 'int', hidden: false, useNull: true },
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'BaseClients',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    },
    getEvents: function (d) {
        var f = this.getId() || this.internalId;
        var c = [];
        if (d) {
            for (var b = 0, a = d.getCount() ; b < a; b++) {
                var e = d.getAt(b);
                var bcids = e.data['BaseClientTreeIds'];
                if (bcids) {
                    var bIDs = bcids.split('|');
                    if (bIDs.indexOf(f.toString()) >= 0) {
                        c.push(e);
                    }
                }
            }
        }
        return c;
    }
});