#Multimedia Interfaces
#-Exploring SDKS and APIs
##Soundcloud
###REGISTER APPLICATION HERE: http://soundcloud.com/you/apps
*“Note if you are going to stream from our API you need to attribute properly.* *Make sure you've read our Terms and Attribution Guidelines to make sure you* *treat our creators content correctly. When using a custom player you must:*

*1.  Credit the uploader as the creator of the sound*
*2.  Credit SoundCloud as the source by including one of the logos found here:**https://developers.soundcloud.com/docs/api/buttons-logos*
*3.  Link to the SoundCloud URL containing the work*
*4.  If the sound is private link to the profile of the creator“*

####Notes
•   SDKS are available for Python, Ruby, PHP, iOS, and Javascript
•   Can embed Soundcloud Widget (not intenteded plan) OR use the JavaScript SDK to stream audio content in the browser
•   Your app can take an audio file and upload it to a user's SoundCloud account

•   All SoundCloud resources are accessed and manipulated in a similar way. A list of the latest resource is usually available through /[resource name], a single specific resource through /[resource name][id] and related subresources like a tracks comments through /[resource name]/[id]/[subresource name]. 

•   Resources are returned as XML by default, or JSON if a .json extension is appended to the resource URI. We encourage you to use JSON. You can also send an appropriate Accept header specifying the format you would like. For example, a request with the header Accept: application/json will return resources represented as a JSON document.

####Code

#####Initialising the Soundcloud SDK with own client ID
```js
<script src="//connect.soundcloud.com/sdk.js"></script>
<script>
  SC.initialize({
    client_id: "YOUR_CLIENT_ID",
    redirect_uri: "http://example.com/callback.html",
  });
</script>
```
#####Streaming a sound
```js
<script src="http://connect.soundcloud.com/sdk.js"></script>
<script>
SC.initialize({
  client_id: 'YOUR_CLIENT_ID'
});

// stream track id 293
SC.stream("/tracks/293", function(sound){
  sound.play();
});
</script>
```

#####Stop track: `SC.recordStop`
Stops the current recording or playback.

#####Searching for a song
Can search for songs from the fields: title, username and description using keywords according to the resource type. They can be filtered by ‘license’, ‘duration’ or ‘tag_list’ (#DnB, #Hardcore #Jazz, etc.)
```js
<script src="http://connect.soundcloud.com/sdk.js"></script>
<script>
SC.initialize({
  client_id: 'YOUR_CLIENT_ID'
});
// find all sounds of buskers licensed under 'creative commons share alike'
SC.get('/tracks', { q: 'buskers', license: 'cc-by-sa' }, function(tracks) {
  console.log(tracks);
});
</script>
```
Songs can also be searched by BPM, duration, etc.
```js
<script src="http://connect.soundcloud.com/sdk.js"></script>
<script>
SC.initialize({
  client_id: 'YOUR_CLIENT_ID'
});

// find all tracks with the genre 'punk' that have a tempo greater than 120 bpm.
SC.get('/tracks', { genres: 'punk', bpm: { from: 120 } }, function(tracks) {
  console.log(tracks);
});
</script>
```
COMPLETE LIST OF SEARCH FIELDS/FILTERS FOUND IN [API REFERENCE](https://developers.soundcloud.com/docs/api/reference#connect)

#####Sharing a song to social media
```js
<script src="http://connect.soundcloud.com/sdk.js"></script>
<script>
SC.initialize({
  client_id: 'YOUR_CLIENT_ID',
  redirect_uri: 'YOUR_REDIRECT_URI'
});

// list connected social networks
SC.connect(function() {
  SC.get('/me/connections', function(connection) {
    alert('Connection id: ' + connection.id);
  });
});
</script>
```
#####Accessing Playlists
```js
<script src="http://connect.soundcloud.com/sdk.js"></script>
<script>
SC.initialize({
  client_id: 'YOUR_CLIENT_ID'
});

SC.get('/playlists/1234323', function(playlist) {
  for (var i = 0; i < playlist.tracks.length; i++) {
    console.log(playlist.tracks[i].length);
  }
});
</script>
```
#####Follow a User
```js
In order to follow a user you use the “/me/followings” endpoint.
<script src="http://connect.soundcloud.com/sdk.js"></script>
<script>
SC.initialize({
  client_id: 'YOUR_CLIENT_ID',
  redirect_uri: 'YOUR_REDIRECT_URI'
});

SC.connect(function() {
  // Follow user with ID 3207
  SC.put('/me/followings/3207');

  // Unfollow the same user
  SC.delete('/me/followings/3207');
);
</script>
```
#####Like a track    
In order to like a song or playlist you use the “/me/favorites” endpoint.
```js
<script src="http://connect.soundcloud.com/sdk.js"></script>
<script>
SC.initialize({
  client_id: 'YOUR_CLIENT_ID',
  redirect_uri: 'YOUR_REDIRECT_URI'
});

SC.connect(function() {
  // favorite the track with id 43314655
  SC.put('/me/favorites/43314655');
});
</script>
```

##Deezer
Requests
You may use ordinary HTTP GET messages. The base URL for each API method looks like the following 

https://api.deezer.com/version/service/id/method/?parameters
Formats: JSON/JSONP/XML/PHP
Encoding: All requests and responses must be in UTF-8
Deezer API is a RESTful API, it means you have to use GET HTTP request in order to get informations, POST HTTP request to update/add datas and DELETE HTTP request to delete them.
