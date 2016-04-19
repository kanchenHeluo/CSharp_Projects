param($installPath, $toolsPath, $package, $project)
$DTE.ItemOperations.Navigate("https://microsoft.sharepoint.com/teams/ECITNuGet/?package=" + $package.Id + "&version=" + $package.Version)
