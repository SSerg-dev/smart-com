Ext.define('Ext.ux.data.BreezeEntityManager', function () {
    var entityManager;

    return {
        singleton: true,

        getEntityManager: function () {
            var separator = location.href[location.href.length - 1] === '/' ? '' : '/';
            var url = location.href + separator + 'odata';
            return entityManager || (entityManager = new breeze.EntityManager(url));
        }

    };

});