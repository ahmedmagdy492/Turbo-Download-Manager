window.addEventListener('load', function() {
	videos = document.querySelectorAll('video');
	[...videos].forEach(v => {
		if((v.src && !v.src.startsWith('blob:')) ) {
			const a = document.createElement('a');
			a.href = v.src;
			a.innerHTML = "Download this Video";
			a.download = v.src;
			a.style.background = 'black'
			a.style.color = 'white';
			a.style.padding = "15px";
			a.style.position = 'absolute';
			a.style.zIndex = '10000';
			a.style.borderRadius = '10px';
			if(v.parentElement) {
				v.parentElement.appendChild(a);
			}
			else {
				v.appendChild(a);
			}
		}
		else if((v.children && v.children.length > 0)) {
			const sources = v.querySelectorAll('source');
			if(sources && sources.length > 0) {
				const a = document.createElement('a');
				const sourcesArr = [...sources].filter(s => s.src.endsWith('.mp4'));
				a.href = sourcesArr[0].src;
				a.innerHTML = "Download this Video";
				a.download = decodeURI(new URL(sourcesArr[0].src).pathname);
				a.style.background = '#FFC800'
				a.style.color = '#000';
				a.style.padding = "5px";
				a.style.margin = '5px';
				a.style.border = '3px solid #000';
				a.style.textDecoration = 'none';
				a.style.borderRadius = '10px';
				a.style.position = 'absolute';
				a.style.zIndex = '10000';
				a.style.position = 'absolute';
				a.style.top = '10px;'
				a.style.left = '0';
				if(v.parentElement) {
					v.parentElement.style.position = 'relative';
					v.parentElement.appendChild(a);
				}
				else {
					v.style.position = 'relative';
					v.appendChild(a);
				}
			}
		}
	});
});