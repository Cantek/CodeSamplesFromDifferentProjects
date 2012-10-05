<script type="text/x-handlebars" data-template-name="games">
<div {{bindAttr class="overview:hide :span12"}}>
	<div class="well game-iframe">
		<iframe width="760" frameborder="0" scrolling="no" {{bindAttr src="game.integration_iframe" height="game.height"}} class="controls"></iframe>
	</div>
</div>
<div {{bindAttr class="single:hide"}}>
	<div class="span8">
		<div class="row">
			<div class="span8">
				{{view Platform.viewGameCarousel gamesBinding="games"}}
			</div>
		</div>
		<div class="row">
			<div class="span8">
				<div class="well">
				<ul class="thumbnails promo">
					{{#each games}}
					<li class="span4 game-promo promo-medium">
						<a {{action "showGame" on="click"}} href="#" class="thumbnail">
							<img {{bindAttr src="promoMedium"}}>
							<div class="title">
								{{title}}
							</div>
							<div class="info">
									{{description}}
							</div>

								<div class="users">
									<div class="user_count_text">
										<i class="icon-user"></i>
										<span class="user_count">{{user_count}}</span>
									</div>
									<div class="btn btn-info btn-play-promo">Play</div>
								</div>

						</a>
					</li>
					{{/each}}
				</ul>
				</div>
			</div>
		</div>
	</div>

	<div class="span4">
		<div class="row">
			<div class="well span4">
				<div class="aboutSidebar"> 
					<div class="partnersText">
						ABOUT
					</div>
					<div class="aboutText">Play the best online games for free with friends that you can easily find through our friend engine. <span style="font-weight:600;">The fun starts here!</span></div>
					<div class="players">
						<div class="number"><span style="font-weight:600;">+28 Million</span> Players Every Month</div>
					</div>
				</div>
			</div>
		</div>
		<div class="row">
			<div class="well span4">
				<div class="partnersText">
					TODAY'S SPECIAL
				</div>
				<center>
				<div class="sidebarPromo">
					<div class="title">
						Harvest Festival!
					</div>
					<div class="promo_content">
						<img src="assets/img/platform/sidepromo.png">
					</div>
					<a href="#games/2/playOnline">
					<div class="btn btn-info btnplinga_redeem">
						<div class="redeem_text">GO TO GAME</div>
					</div>
					</a>
					<div class="promo_text">
						<span style="font-weight:600;">New harvest items</span>  available now in Family Barn.
					</div>
				</div>
				</center>
			</div>
		</div>
		
		<div class="row">
			<div class="well span4">
				<div class="partnersSidebar"> 
					<div class="partnersText">
						PARTNERS
					</div>
					<a href="/partner" class="btn-publish-href">
						<div class="btn btn-publish">
							<div class="publish_text">
								Publish our Games
							</div>
						</div>
					</a>
					<div class="partnersContentText">Embed our games into your site and start earning instantly. Great monatization and transparent reporting.
					</div>
				</div>
			</div>
		</div>
	</div>
</div>
</script>
<script type="text/x-handlebars" data-template-name="gameCarousel">
<div id="gameCarousel" class="carousel slide well">
	<div class="carousel-inner">
		{{#each games}}
			<div {{bindAttr class="active :item"}} {{action "showGame" on="click"}}>
				<img {{bindAttr src="hotbox"}} width="580px" height="400px" alt>
				<div class="carousel-caption">
					<p>{{description}}</p>
					<button class="btn btn-large btn-danger" href="#">Play now</button>
				</div>
			</div>
		{{/each}}
	</div>
	<a class="carousel-control left" href="#gameCarousel" data-slide="prev">&lsaquo;</a>
	<a class="carousel-control right" href="#gameCarousel" data-slide="next">&rsaquo;</a>
</div>
</script>