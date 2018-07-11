var Better4YouItemManagment = null;

//function DocumentReady() {
    Better4YouItemManagment = Ember.Application.create({
        rootElement: '#emberBody',
    });

    Better4YouItemManagment.ApplicationController = Ember.Controller.extend();
    Better4YouItemManagment.ApplicationView = Ember.View.extend({
        templateName: "application"
    });


//}

Better4YouItemManagment.Router.map(function () {
    this.resource("index", { path: "/" });
    this.resource("create", { path: "create/" });
    this.resource("detail", { path: "detail/:item_id" });
});

Better4YouItemManagment.IndexController = Ember.ArrayController.extend({
    content: []
});
Better4YouItemManagment.IndexRoute = Ember.Route.extend({
    model: function () {
        return Better4YouItemManagment.Food.find();
    },
    events: {
        gotoDetail: function (model) {
            this.transitionTo('detail',model);
        }
    }
    /*,
    setupController:function (controller,model) {
        controller.set('content', model);
    }*/
});
Better4YouItemManagment.IndexView = Ember.View.extend({
    templateName: "index"
});

/* Begin Detail Part */
Better4YouItemManagment.DetailView = Ember.View.extend({
    templateName: "detail",
});
Better4YouItemManagment.DetailController = Ember.ObjectController.extend({
    content: null,
    foodTypes: [],
    ingredientTypes: [],
    fieldIds: [],
    fieldValues: {},
    isEditing: false,
    output:null,
    edit:function (model) {
        return this.set('isEditing', true);
    },
    doneEditing:function(){
        return this.set('isEditing', false);
    }
});
Better4YouItemManagment.DetailRoute = Ember.Route.extend({
    model: function (params) {
        return Better4YouItemManagment.Food.find(params.item_id);
    },
    setupController: function (controller, model) {
        controller.set('foodTypes', Better4YouItemManagment.FoodType.find());
        controller.set('fieldIds', Better4YouItemManagment.IngredientType.find());
        controller.set('content', model);
        var fieldValues = {};
        var aa = model.get('foodIngredients');
        //var output = "";
        aa.map(function (foodIngredient) {
            //var foodIng = Better4YouItemManagment.FoodIngredient.find(foodIngredient.id);
            fieldValues[foodIngredient.get('ingredientType.id')] = foodIngredient.get('value');
            //fieldValues += foodIng.get('ingredientType') + "," + foodIng.get('value');
        });
        /*
        $.each(aa,function (itemIndex,foodIngredient) {

        //model.get('foodIngredients').map(function (foodIngredient) {
        //$.each(model.get('foodIngredients'),function(foodIngredient) {
            //fieldValues[foodIngredient.ingredientType] = foodIngredient.value;          
            fieldValues[foodIngredient.ingredientType.id] =foodIngredient.value;
            //var foodIngredientItem = Better4YouItemManagment.FoodIngredient.find(foodIngredient.id);
            //fieldValues.set(foodIngredientItem.ingredientType, foodIngredientItem.value);
        });
        */
        controller.set('fieldValues', fieldValues);
    }
});
/* End Detail Part */

/* Begin Detail Part */
Better4YouItemManagment.CreateView = Ember.View.extend({
    templateName: "create"//,
    //create: function (model) {
    //    alert(model.get("foodType"));
    //    alert(model.get("name"));
    //    var fieldValues = this.get('fieldValues');
    //    var output = this.get('fieldIds').map(function (name) {
    //        return name + ": " + fieldValues[name];
    //    });
    //    this.set('output', output.join(', '));
    //}
});
Better4YouItemManagment.CreateController = Ember.ObjectController.extend({
    content: null,    
    foodTypes: [],
    ingredientTypes: [],
    fieldIds: [],
    fieldValues: {},
    //selectedFoodType: null,
    create: function (model) {
        //http://jsfiddle.net/PXnWj/
        //http://stackoverflow.com/questions/15162869/ember-textfield-valuebinding-with-dynamic-property
        //var foodModel = Better4YouItemManagment.Food.createRecord({name:model.get('name'),foodType:model.get('foodType')});
        var fieldValues = this.get('fieldValues');
        this.get('fieldIds').map(function (name) {
            if (fieldValues[name.id] != undefined)
            {
                Better4YouItemManagment.FoodIngredient.createRecord({ ingredientType: name, value: fieldValues[name.id], food: model });
                    //var foodIngredient = Better4YouItemManagment.FoodIngredient.createRecord({ ingredientType: name, value: fieldValues[name.id], food: model });
                    //model.get("foodIngredients").pushObject(foodIngredient);
            }
        });
        this.get('store').commit();
        this.set('fieldValues', { });
        this.transitionTo('index');
    }
});
Better4YouItemManagment.CreateRoute = Ember.Route.extend({
    
    model: function (params) {
        var model = Better4YouItemManagment.Food.createRecord();
        //var test = Better4YouItemManagment.IngredientType.find();
        //test.forEach(function(item) { alert(item.id); });
        //var foodIngredient = Better4YouItemManagment.FoodIngredient.createRecord();
        //foodIngredient.food = model;
        return model;
    },    
    setupController: function (controller, model) {        
        controller.set('foodTypes', Better4YouItemManagment.FoodType.find());
        //controller.set('ingredientTypes', Better4YouItemManagment.IngredientType.find());
        controller.set('fieldIds', Better4YouItemManagment.IngredientType.find());
        controller.set('content', model);
    }
});
/* End Detail Part */

Better4YouItemManagment.Store = DS.Store.extend({
    revision: 11,
    adapter: 'DS.FixtureAdapter'
    //adapter : DS.FixtureAdapter.extend({
    //    simulateRemoteResponse: true
    //})
    /*
    adapter: DS.DjangoRESTAdapter.create({
        namespace:'codecamp'
    })
    */
});
Better4YouItemManagment.Food = DS.Model.extend({
    name: DS.attr('string'),
    foodType: DS.belongsTo('Better4YouItemManagment.FoodType'),
    foodIngredients: DS.hasMany('Better4YouItemManagment.FoodIngredient')
});

Better4YouItemManagment.Food.FIXTURES = [
    { id: 1, name: "food1", foodType: 106001, foodIngredients: [1, 2, 3, 4, 5] },
    { id: 2, name: "food2", foodType: 106002, foodIngredients: [6, 7, 8, 9, 10] },
    { id: 3, name: "food3", foodType: 106003, foodIngredients: [11, 12, 13, 14, 15] },
    { id: 4, name: "food4", foodType: 106004, foodIngredients: [16, 17, 18, 19, 20] }
];
Better4YouItemManagment.FoodIngredient = DS.Model.extend({
    description: DS.attr('string'),
    value: DS.attr('string'),
    food: DS.belongsTo('Better4YouItemManagment.Food'),
    ingredientType: DS.belongsTo('Better4YouItemManagment.IngredientType')
});

Better4YouItemManagment.FoodIngredient.FIXTURES = [
    { id: 1, description: "test food1", value: 1, food: 1, ingredientType: 109001 },
    { id: 2, description: "test food2", value: 1, food: 1, ingredientType: 109002 },
    { id: 3, description: "test food3", value: 1, food: 1, ingredientType: 109003 },
    { id: 4, description: "test food4", value: 1, food: 1, ingredientType: 109004 },
    { id: 5, description: "test food5", value: 1, food: 1, ingredientType: 109005 },
    { id: 6, description: "test food1", value: 1, food: 2, ingredientType: 109001 },
    { id: 7, description: "test food2", value: 1, food: 2, ingredientType: 109002 },
    { id: 8, description: "test food3", value: 1, food: 2, ingredientType: 109003 },
    { id: 9, description: "test food4", value: 1, food: 2, ingredientType: 109004 },
    { id: 10, description: "test food5", value: 1, food: 2, ingredientType: 109005 },
    { id: 11, description: "test food1", value: 1, food: 3, ingredientType: 109001 },
    { id: 12, description: "test food2", value: 1, food: 3, ingredientType: 109002 },
    { id: 13, description: "test food3", value: 1, food: 3, ingredientType: 109003 },
    { id: 14, description: "test food4", value: 1, food: 3, ingredientType: 109004 },
    { id: 15, description: "test food5", value: 1, food: 3, ingredientType: 109005 },
    { id: 16, description: "test food1", value: 1, food: 4, ingredientType: 109001 },
    { id: 17, description: "test food2", value: 1, food: 4, ingredientType: 109002 },
    { id: 18, description: "test food3", value: 1, food: 4, ingredientType: 109003 },
    { id: 19, description: "test food4", value: 1, food: 4, ingredientType: 109004 },
    { id: 20, description: "test food5", value: 1, food: 4, ingredientType: 109005 }
];

Better4YouItemManagment.FoodType = DS.Model.extend({
    name: DS.attr('string'),
});

Better4YouItemManagment.FoodType.FIXTURES = [
{ id: 106001, name: 'Milk'},
{ id: 106002, name: 'Fruit'},
{ id: 106003, name: 'Grain'},
{ id: 106004, name: 'Meat Alternative'}
];

Better4YouItemManagment.IngredientType = DS.Model.extend({
    name: DS.attr('string'),
});

Better4YouItemManagment.IngredientType.FIXTURES = [
{ id: 109001, name: 'Portion Size'},
{ id: 109002, name: 'Calories'},
{ id: 109003, name: 'Cholesterol'},
{ id: 109004, name: 'Sodium'},
{ id: 109005, name: 'Fiber'},
{ id: 109006, name: 'Iron'},
{ id: 109007, name: 'Calcium'},
{ id: 109008, name: 'Vitamin A'},
{ id: 109009, name: 'Vitamin B'},
{ id: 109010, name: 'Vitamin C'},
{ id: 109011, name: 'Protein'},
{ id: 109012, name: 'Carbs'},
{ id: 109013, name: 'Total Fat'},
{ id: 109014, name: 'Sat Fat'},
{ id: 109015, name: 'Trans Fat'}
];


Better4YouItemManagment.TextField = Ember.TextField.extend({
    fieldId: null,
    values: null,

    updateValues: function () {
        var fieldId = this.get('fieldId');
        var values = this.get('values');
        if (values && fieldId) values[fieldId.id] = this.get('value');
    }.observes('value')
});