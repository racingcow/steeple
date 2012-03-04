function PushToGitHub
{
	param([string]$repoPath)
	cd $repoPath
	git push origin master --v
}

PushToGitHub $args[0]