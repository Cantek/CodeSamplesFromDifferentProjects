Partner.controllerUser = Ember.Object.create({
	m : Partner.controllerMain
	errorPassword : false
	errorCompanyname : false
	errorUsername : false
	errorEmail : false
	errorLogin : false
	errorRegister : false
	errorUsernameMessage : ''
	errorPasswordMessage : ''
	errorEmailMessage : ''
	errorCompanynameMessage : ''
	errorPlatformAddition : ''
	errorPlatformBackendName : ''
	errorPlatformName : ''
	errorPlatformUrl : ''
	errorPlatformBackendNameMessage : ''
	errorPlatformNameMessage : ''
	errorPlatformUrlMessage : ''
	editEmail : false
	editUrl : false
	editPlatformName : false
	editCompanyname : false
	editPassword : false
	settingsChangeError:false
	settingsChangeErrorText:''
	init : () ->
		@_super()
		@m.toIndex()
		@m.getRequest('/partner/status', {}, (response) =>
			@setDefaultParams(response)
			@m.toIndex()
			if response.authenticated
				@setUser(response)
		)
	
	current : Ember.Object.create({
		loaded: false,
		authenticated: false,
		partnerId: '',
		url:'',
		platforms:[],
		platform:'',
		notAuthenticated : Em.computed(() -> !@get('authenticated')).property('authenticated')
		
		setData : (data) ->
			for own item, index of data
				@set(item, index)
	})
	
	login : (data) ->
		if data.remember?
			data.remember = true
		else
			data.remember = false
			
		# name, password, remember
		@m.postRequest('/partner/login', data, (response) =>
			if response.authenticated
				@set('errorLogin', false)
				@setUser(response)
				Partner.controllerGames.set 'platforms', response.platforms
			else 
				@set('errorLogin', true)
		)

	setCurrentPlatform : (id) ->
		user = @get('current')
		platforms = user.platforms
		console.log('Current platform: ' + id + ' Platforms: ' + platforms.length)
		for platform in platforms
			if platform.id is id
				@set('platform', platform)
				console.log('Current platform setted.')
	
	setDefaultParams : (response) ->
		@m.set('resultMessage', response.resultMessage);
		
	setUser : (response) ->
		user = @get('current')
		user.set('authenticated', true)
		user.setData(response.user)
		@set('current', user)
		
	logout : () ->
		user = @get('current')
		user.set('authenticated', false)
		@set('current', user)
		@m.getRequest('/partner/logout')
		
	changeAPIKey : () ->
		@m.getRequest('/partner/change_api_key', {platform_id: Partner.controllerUser.current.platform.id}, (response) =>
			user = @get('current')
			user.set('api_key', response.api_key)
			@set('current', user)
		)

	changeData:(data)->
		text = "";
		@m.postRequest('/partner/edit',data,(response) =>
			user = @get('current')
			if	response.authenticated
				user.setData(response.user)
				@set 'settingsChangeError', false
				Partner.controllerUser.set 'editEmail',false
				Partner.controllerUser.set 'editCompanyname',false
				Partner.controllerUser.set 'editUrl',false
				Partner.controllerUser.set 'editPassword',false		
				$('#enterPassword').modal('hide')
				@set('current',user)
			else
				response.errors.forEach((error) =>
					text += error.message+"\n";
				)
				@set 'settingsChangeError', true
				@set 'settingsChangeErrorText', text
			)
	changePlatformData:(data)->
		text = "";
		@m.postRequest('/partner/edit_platform',data,(response) =>
			user = @get('current')
			if	response.authenticated
				user.setData(response.user)
				@set 'settingsChangeError', false
				Partner.controllerUser.set 'editPlatformName',false
				Partner.controllerUser.set 'editUrl',false
				$('#enterPassword').modal('hide')
				@set('current',user)
			else
				response.errors.forEach((error) =>
					text += error.message+"\n";
				)
				@set 'settingsChangeError', true
				@set 'settingsChangeErrorText', text
			)
	register : (data) ->
		$('#register').bind('hidden', () =>
						@set('errorRegister', false)
						@set('errorUsername', false)
						@set('errorPassword', false)
						@set('errorCompanyname',false)
						@set('errorEmail', false)
				)
	
		@m.postRequest('/partner/register', data, (response) =>
			@set('errorRegister', false)
			@set('errorUsername', false)
			@set('errorPassword', false)
			@set('errorCompanyname',false)
			@set('errorEmail', false)
			
			if response.status is "ok"
				$('#register').modal('hide')
				user = @get('current')
				user.set('authenticated', true)
				user.setData(response.user)
				@set('current', user)
			else
				@set('errorRegister', true)
				response.errors.forEach((error) =>
					switch error.error
						when "username"
							@set('errorUsername', true)
							@set('errorUsernameMessage',error.message)
						when "company_name"
							@set('errorCompanyname', true)
							@set('errorCompanynameMessage',error.message)
						when "password"
							@set('errorPassword', true)
							@set('errorPasswordMessage',error.message)
						when "email"
							@set('errorEmail', true)
							@set('errorEmailMessage',error.message)
				)
			)

	addPlatform	: (data) ->
		$('#addPlatform').bind('hidden',() =>
						@set('errorPlatformAddition', false)
						@set('errorPlatformBackendName', false)
						@set('errorPlatformName',false)
						@set('errorPlatformUrl', false)
						)
		@m.postRequest('/partner/create_a_partner', data, (response) =>
			@set('errorPlatformAddition', false)
			@set('errorPlatformBackendName', false)
			@set('errorPlatformName', false)
			@set('errorPlatformUrl', false)

			if response.status is "ok"
				$('#addPlatform').modal('hide')
				user = @get('current')
				user.set('authenticated', true)
				user.setData(response.user)
				@set('current', user)
				Partner.routeManager.set 'location', 'platforms'
			else
				@set('errorPlatformAddition', true)
				response.errors.forEach((error) =>
					switch error.error
						when "platformBackendName"
							@set('errorPlatformBackendName', true)
							@set('errorPlatformBackendNameMessage',error.message)
						when "platformName"
							@set('errorPlatformName', true)
							@set('errorPlatformNameMessage',error.message)
						when "platformUrl"
							@set('errorPlatformUrl', true)
							@set('errorPlatformUrlMessage',error.message)
				)
			)
})